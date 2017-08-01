var gulp = require("gulp");
var msbuild = require("gulp-msbuild");
var debug = require("gulp-debug");
var foreach = require("gulp-foreach");
var rename = require("gulp-rename");
var watch = require("gulp-watch");
var merge = require("merge-stream");
var newer = require("gulp-newer");
var util = require("gulp-util");
var runSequence = require("run-sequence");
var path = require("path");
var config = require("./gulp-config.js")();
var nugetRestore = require('gulp-nuget-restore');
var fs = require('fs');
var unicorn = require("./scripts/unicorn.js");
var habitat = require("./scripts/habitat.js");
var yargs = require("yargs").argv;

module.exports.config = config;

gulp.task("default", function (callback) {
  config.runCleanBuilds = true;
  return runSequence(
    "01-Copy-Sitecore-License",
    "02-Nuget-Restore",
    "03-Publish-All-Projects",
    "04-Apply-Xml-Transform",
    "05-Sync-Unicorn",
	callback);
});

gulp.task("buildonly", function (callback) {
  config.runCleanBuilds = true;
  return runSequence(
    "01-Copy-Sitecore-License",
    "02-Nuget-Restore",
    "03-Publish-All-Projects",
    "04-Apply-Xml-Transform",
	callback);
});

gulp.task("resources", function (callback) {
  config.runCleanBuilds = true;
  return runSequence(
    "Publish-Global-CSS",
    "Publish-Global-less",
    "Publish-Global-sass",
    "Publish-Global-Scripts",
	callback);
});

gulp.task("habitatDefault", function (callback) {
  config.runCleanBuilds = true;
  return runSequence(
    "01-Copy-Sitecore-License",
    "02-Nuget-Restore",
    "03-Publish-All-Projects",
    "04-Apply-Xml-Transform",
    "05-Sync-Unicorn",
    "06-Deploy-Transforms",
	callback);
});

gulp.task("deploy", function (callback) {
  config.runCleanBuilds = true;
  return runSequence(
    "01-Copy-Sitecore-License",
    "02-Nuget-Restore",
    "03-Publish-All-Projects",
    "04-Apply-Xml-Transform",
	"06-Deploy-Transforms",
	callback);
});

/*****************************
  Initial setup
*****************************/
gulp.task("01-Copy-Sitecore-License", function () {
  console.log("Copying Sitecore License file from configured Source to Solution lib directory");
  //license file exists in gulp-config
  //gulp.dest - will copy to "lib" folder in solution directory.
  //not sure why this is needed?
  return gulp.src(config.licensePath).pipe(gulp.dest("./lib"));
});

gulp.task("02-Nuget-Restore", function (callback) {
  var solution = "./" + config.solutionName + ".sln";
  console.log("Running nugetRestore Command on sln: " + solution);
  return gulp.src(solution).pipe(nugetRestore());
});

gulp.task("03-Publish-All-Projects", function (callback) {
  //First Step Cleans and Builds entire solution
  //Later Steps will do a project by project build
  //The ouptut of each build will be in a "obj/package" folder 
  //within the project and then copied to website root
  //note when I first tried to get this to work, I had errors
  //due to Habitat project files being in the features folder
  //not sure if I fat fingered it, or yeoman set it up with
  //the default scafolding. Once cleaned up, it worked fine.
  return runSequence(
    "Build-Solution",
    "Publish-Foundation-Projects",
    "Publish-Feature-Projects",
    "Publish-Project-Projects", callback);
});

gulp.task("04-Apply-Xml-Transform", function () {
  //Currently don't have any transforms.
  var layerPathFilters = ["./src/Foundation/**/*.transform", "./src/Feature/**/*.transform", "./src/Project/**/*.transform", "!./src/**/obj/**/*.transform", "!./src/**/bin/**/*.transform"];
  return gulp.src(layerPathFilters)
    .pipe(foreach(function (stream, file) {
      var fileToTransform = file.path.replace(/.+code\\(.+)\.transform/, "$1");
      util.log("Applying configuration transform: " + file.path);
      return gulp.src("./scripts/applytransform.targets")
        .pipe(msbuild({
          targets: ["ApplyTransform"],
          configuration: config.buildConfiguration,
          logCommand: false,
          verbosity: "minimal",
          stdout: true,
          errorOnFail: true,
          maxcpucount: 0,
          toolsVersion: config.buildToolsVersion,
          properties: {
            Platform: config.buildPlatform,
            WebConfigToTransform: config.websiteRoot,
            TransformFile: file.path,
            FileToTransform: fileToTransform
          }
        }));
    }));
});

gulp.task("05-Sync-Unicorn", function (callback) {
  //This is dependent on Unicorn being installed on your instance
  //If this is your main solution, you just need to add a reference to Unicorn using Nuget
  //This wasn't clear to me at first, but I followed what habitat did and created
  //A standalone Foundation project named Serialization to include this reference/configuration.
  //Also note, I was getting an error after deploying the Rainbow.config file in the Serialization project.
  //Could not get pipeline: preprocessRequest
  //To resolve, I downgraded the version of Unicorn and Rainbow to the versions that ship with the Habitat Project
  var options = {};
  options.siteHostName = habitat.getSiteUrl();
  console.log('Syncing Unicorn to ' + options.siteHostName);
  options.authenticationConfigFile = config.websiteRoot + "/App_config/Include/Unicorn/Unicorn.UI.config";

  unicorn(function() { return callback() }, options);
});


gulp.task("06-Deploy-Transforms", function () {
  return gulp.src("./src/**/code/**/*.transform")
      .pipe(gulp.dest(config.websiteRoot + "/temp/transforms"));
});

/*****************************
  Copy assemblies to all local projects
*****************************/
gulp.task("Copy-Local-Assemblies", function () {
  console.log("Copying site assemblies to all local projects");
  var files = config.sitecoreLibraries + "/**/*";

  var root = "./src";
  var projects = root + "/**/code/bin";
  return gulp.src(projects, { base: root })
    .pipe(foreach(function (stream, file) {
      console.log("copying to " + file.path);
      gulp.src(files)
        .pipe(gulp.dest(file.path));
      return stream;
    }));
});

/*****************************
  Publish
*****************************/
var publishStream = function (stream, dest) {
  var targets = ["Build"];

  return stream
    .pipe(debug({ title: "Building project:" }))
    .pipe(msbuild({
      targets: targets,
      configuration: config.buildConfiguration,
      logCommand: false,
      verbosity: "minimal",
      stdout: true,
      errorOnFail: true,
      maxcpucount: 0,
      toolsVersion: config.buildToolsVersion,
      properties: {
        Platform: config.publishPlatform,
        DeployOnBuild: "true",
        DeployDefaultTarget: "WebPublish",
        WebPublishMethod: "FileSystem",
        DeleteExistingFiles: "false",
        publishUrl: dest,
        _FindDependencies: "false"
      }
    }));
}

var publishProject = function (location, dest) {
  dest = dest || config.websiteRoot;

  console.log("publish to " + dest + " folder");
  return gulp.src(["./src/" + location + "/code/*.csproj"])
    .pipe(foreach(function (stream, file) {
      return publishStream(stream, dest);
    }));
}

var publishProjects = function (location, dest) {
  dest = dest || config.websiteRoot;

  console.log("publish to " + dest + " folder");
  return gulp.src([location + "/**/code/*.csproj"])
    .pipe(foreach(function (stream, file) {
      return publishStream(stream, dest);
    }));
};

gulp.task("Build-Solution", function () {
  //Do a clean, or clean and build
  var targets = ["Build"];
  if (config.runCleanBuilds) {
    targets = ["Clean", "Build"];
  }

  var solution = "./" + config.solutionName + ".sln";
  console.log("running MSBuild on Solution: " + solution);
  return gulp.src(solution)
      .pipe(msbuild({
          targets: targets,
          configuration: config.buildConfiguration,
          logCommand: false,
          verbosity: "minimal",
          stdout: true,
          errorOnFail: true,
          maxcpucount: 0,
          toolsVersion: config.buildToolsVersion,
          properties: {
            Platform: config.buildPlatform
          }
        }));
});

gulp.task("Publish-Foundation-Projects", function () {
  return publishProjects("./src/Foundation");
});

gulp.task("Publish-Feature-Projects", function () {
  return publishProjects("./src/Feature");
});

gulp.task("Publish-Project-Projects", function () {
  return publishProjects("./src/Project");
});

gulp.task("Publish-Project", function () {
  if(yargs && yargs.m && typeof(yargs.m) == 'string') {
    return publishProject(yargs.m);
  } else {
    throw "\n\n------\n USAGE: -m Layer/Module \n------\n\n";
  }
});

gulp.task("Publish-Assemblies", function () {
  var root = "./src";
  var binFiles = root + "/**/code/**/bin/Sitecore.{Feature,Foundation,Habitat}.*.{dll,pdb}";
  var destination = config.websiteRoot + "/bin/";
  return gulp.src(binFiles, { base: root })
    .pipe(rename({ dirname: "" }))
    .pipe(newer(destination))
    .pipe(debug({ title: "Copying " }))
    .pipe(gulp.dest(destination));
});

gulp.task("Publish-All-Views", function () {
  var root = "./src";
  var roots = [root + "/**/Views", "!" + root + "/**/obj/**/Views"];
  var files = "/**/*.cshtml";
  var destination = config.websiteRoot + "\\Views";
  return gulp.src(roots, { base: root }).pipe(
    foreach(function (stream, file) {
      console.log("Publishing from " + file.path);
      gulp.src(file.path + files, { base: file.path })
        .pipe(newer(destination))
        .pipe(debug({ title: "Copying " }))
        .pipe(gulp.dest(destination));
      return stream;
    })
  );
});

gulp.task("Publish-All-Configs", function () {
  var root = "./src";
  var roots = [root + "/**/App_Config", "!" + root + "/**/obj/**/App_Config"];
  var files = "/**/*.config";
  var destination = config.websiteRoot + "\\App_Config";
  return gulp.src(roots, { base: root }).pipe(
    foreach(function (stream, file) {
      console.log("Publishing from " + file.path);
      gulp.src(file.path + files, { base: file.path })
        .pipe(newer(destination))
        .pipe(debug({ title: "Copying " }))
        .pipe(gulp.dest(destination));
      return stream;
    })
  );
});

gulp.task("Publish-Global-CSS", function () {
  var root = "./src";
  var roots = [root + "/**/global/css", "!" + root + "/**/obj/**/global/css"];
  var files = "/**/*.css";
  var destination = config.websiteRoot + "\\global\\css";
  return gulp.src(roots, { base: root }).pipe(
    foreach(function (stream, file) {
      console.log("Copying Styles " + file.path);
      gulp.src(file.path + files, { base: file.path })
        .pipe(newer(destination))
        .pipe(debug({ title: "Copying " }))
        .pipe(gulp.dest(destination));
      return stream;
    })
  );
});

gulp.task("Publish-Global-less", function () {
  var root = "./src";
  var roots = [root + "/**/global/less", "!" + root + "/**/obj/**/global/less"];
  var files = "/**/*.less";
  var destination = config.websiteRoot + "\\global\\less";
  return gulp.src(roots, { base: root }).pipe(
    foreach(function (stream, file) {
      console.log("Copying Less " + file.path);
      gulp.src(file.path + files, { base: file.path })
        .pipe(newer(destination))
        .pipe(debug({ title: "Copying " }))
        .pipe(gulp.dest(destination));
      return stream;
    })
  );
});

gulp.task("Publish-Global-sass", function () {
  var root = "./src";
  var roots = [root + "/**/global/sass", "!" + root + "/**/obj/**/global/sass"];
  var files = "/**/*.scss";
  var destination = config.websiteRoot + "\\global\\sass";
  return gulp.src(roots, { base: root }).pipe(
    foreach(function (stream, file) {
      console.log("Copying Sass from " + file.path);
      gulp.src(file.path + files, { base: file.path })
        .pipe(newer(destination))
        .pipe(debug({ title: "Copying " }))
        .pipe(gulp.dest(destination));
      return stream;
    })
  );
});

gulp.task("Publish-Global-Scripts", function () {
  var root = "./src";
  var roots = [root + "/**/global/scripts", "!" + root + "/**/obj/**/global/scripts"];
  var files = "/**/*.js";
  var destination = config.websiteRoot + "\\global\\scripts";
  return gulp.src(roots, { base: root }).pipe(
    foreach(function (stream, file) {
      console.log("Copying Scripts " + file.path);
      gulp.src(file.path + files, { base: file.path })
        .pipe(newer(destination))
        .pipe(debug({ title: "Copying " }))
        .pipe(gulp.dest(destination));
      return stream;
    })
  );
});

/*****************************
 Watchers
*****************************/
gulp.task("Auto-Publish-Css", function () {
  var root = "./src";
  var roots = [root + "/**/styles", "!" + root + "/**/obj/**/styles"];
  var files = "/**/*.css";
  var destination = config.websiteRoot + "\\styles";
  gulp.src(roots, { base: root }).pipe(
    foreach(function (stream, rootFolder) {
      gulp.watch(rootFolder.path + files, function (event) {
        if (event.type === "changed") {
          console.log("publish this file " + event.path);
          gulp.src(event.path, { base: rootFolder.path }).pipe(gulp.dest(destination));
        }
        console.log("published " + event.path);
      });
      return stream;
    })
  );
});

gulp.task("Auto-Publish-Views", function () {
  var root = "./src";
  var roots = [root + "/**/Views", "!" + root + "/**/obj/**/Views"];
  var files = "/**/*.cshtml";
  var destination = config.websiteRoot + "\\Views";
  return gulp.src(roots, { base: root }).pipe(
    foreach(function (stream, rootFolder) {
      gulp.watch(rootFolder.path + files, function (event) {
        if (event.type === "changed") {
          console.log("publish this file " + event.path);
          gulp.src(event.path, { base: rootFolder.path }).pipe(gulp.dest(destination));
        }
        console.log("published " + event.path);
      });
      return stream;
    })
  );
});

gulp.task("Auto-Publish-Assemblies", function () {
  var root = "./src";
  var roots = [root + "/**/code/**/bin"];
  var files = "/**/Sitecore.{Feature,Foundation,Habitat}.*.{dll,pdb}";;
  var destination = config.websiteRoot + "/bin/";
  gulp.src(roots, { base: root }).pipe(
    foreach(function (stream, rootFolder) {
      gulp.watch(rootFolder.path + files, function (event) {
        if (event.type === "changed") {
          console.log("publish this file " + event.path);
          gulp.src(event.path, { base: rootFolder.path }).pipe(gulp.dest(destination));
        }
        console.log("published " + event.path);
      });
      return stream;
    })
  );
});
