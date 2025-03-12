// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


document.addEventListener('DOMContentLoaded', function () {
    const pagosAnchor = document.getElementById('pagos-anchor');
    const dropdownMenu = document.getElementById('dropdown-menu');

    pagosAnchor.addEventListener('click', function (e) {
        e.preventDefault(); // Evita la navegación por defecto
        // Alterna la visibilidad del menú
        dropdownMenu.style.display = (dropdownMenu.style.display === 'none' || dropdownMenu.style.display === '') ? 'block' : 'none';
    });
});