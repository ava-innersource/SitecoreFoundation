define(["sitecore", "/-/speak/v1/ExperienceEditor/ExperienceEditor.js"], function (Sitecore, ExperienceEditor) {
    Sitecore.Commands.LaunchFieldEditor =
     {
         canExecute: function (context) {
             //YOU COULD ADD FUNCTIONALITY HERE TO SEE IF ITEMS HAVE THE CORRECT FIELDS
             return true;
         },
         execute: function (context) {
             context.currentContext.argument = context.button.viewModel.$el[0].accessKey;
             ExperienceEditor.PipelinesUtil.generateRequestProcessor("ExperienceEditor.GenerateFieldEditorUrl", function (response) {
                 var DialogUrl = response.responseValue.value;
                 var dialogFeatures = "dialogHeight: 680px;dialogWidth: 520px;";
                 ExperienceEditor.Dialogs.showModalDialog(DialogUrl, '', dialogFeatures, null);
             }).execute(context);
         }
     };
});