// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

let x = document.querySelectorAll(".money");

for (let i = 0, len = x.length; i < len; i++) {
    let num = Number(x[i].innerHTML).toLocaleString('en');
    x[i].innerHTML = num;
    x[i].classList.add("currSign");
}