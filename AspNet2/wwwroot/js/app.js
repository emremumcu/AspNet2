// Document Ready Implementations
// ******************************

//////$(document).ready(function () {
//////    // ...
//////});

//////jQuery(function() {
//////    // ...
//////});

//////$(function() {
//////    // ...
//////});


/*
 * ***************
 * Document Ready:
 * ***************
 */

$(function ()
{
    FadeOutPreloader();
    SetTheme();  
    
});

/*
 * *********
 * Functions
 * *********
 */

function SetTheme() {    
    var theme = (localStorage.getItem('theme') == null) ? ("default") : (localStorage.getItem('theme'));
    $("link#bootstrap").attr("href", `../css/themes/${theme}/bootstrap.min.css`);    
}

function ChangeTheme(themeName) {    
    localStorage.setItem('theme', themeName);
}

function FadeOutPreloader() {
    $("#preloader").fadeOut("slow");
}