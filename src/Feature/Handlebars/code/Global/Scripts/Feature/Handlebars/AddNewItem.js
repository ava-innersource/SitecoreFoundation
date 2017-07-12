//add new item
document.addEventListener("DOMContentLoaded", function (event) {

    $(".addItemButton").click(function () {
        var template = $(this).attr("data-template");
        var parent = $(this).attr("data-parent");
        var itemName = prompt("Name:");

        console.log("template:" + template);
        console.log("parent:" + parent);
        console.log("itemName:" + itemName);

        if (itemName) {
            $.post("/sitecore/api/sf/additem",
                {
                    name: itemName,
                    templateId: template,
                    parentId: parent
                },
                function (data) {
                    //grab and click the save button to force refresh
                    console.log("success");
                    document.location.reload(true);
                    
                });
        }
    });

});