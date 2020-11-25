// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
function toggle(source) {
    var checkBoxes = document.querySelectorAll("input[name^='Mod.ModuleYear[']");

    for (var i = 0; i < checkBoxes.length; i++) {
        if (checkBoxes[i].type == "checkbox")
            checkBoxes[i].checked = source.checked;
    }
}
