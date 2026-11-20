// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    const input = document.getElementById("busquedaTabla");
    const tabla = document.querySelector(".searchable-table");
    if (!input || !tabla) return;

    // Quita mayúsculas y tildes para que "Dirección" coincida con "direccion"
    const normalizar = (texto) =>
        texto.toLowerCase().normalize("NFD").replace(/[\u0300-\u036f]/g, "");

    const filas = Array.from(tabla.querySelectorAll("tbody tr"));

    // Fila que se muestra cuando no hay coincidencias
    const columnas = tabla.querySelectorAll("thead th").length;
    const sinResultados = document.createElement("tr");
    sinResultados.innerHTML =
        `<td colspan="${columnas}" class="text-center text-muted py-3">No se encontraron resultados</td>`;
    sinResultados.style.display = "none";
    tabla.querySelector("tbody").appendChild(sinResultados);

    input.addEventListener("input", function () {
        const filtro = normalizar(input.value.trim());
        let visibles = 0;

        filas.forEach(function (fila) {
            const coincide = normalizar(fila.textContent).includes(filtro);
            fila.style.display = coincide ? "" : "none";
            if (coincide) visibles++;
        });

        sinResultados.style.display = visibles === 0 ? "" : "none";
    });
});