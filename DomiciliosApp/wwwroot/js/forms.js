/* forms.js — comportamiento de los formularios (Crear / Editar)
   - desplegables con búsqueda (Tom Select)
   - valores por defecto de fecha/hora
   - solo dígitos, limpieza de espacios
   - dirección de entrega que depende del cliente
   - domiciliario -> fecha/hora de asignación; establecimiento <-> dirección externa
*/
(function () {
    "use strict";

    function listo(fn) {
        if (document.readyState !== "loading") fn();
        else document.addEventListener("DOMContentLoaded", fn);
    }

    listo(function () {
        var pad = function (n) { return String(n).padStart(2, "0"); };
        var d = new Date();
        var hoy = d.getFullYear() + "-" + pad(d.getMonth() + 1) + "-" + pad(d.getDate());
        var ahora = pad(d.getHours()) + ":" + pad(d.getMinutes());

        // ---- 1. Fecha / hora por defecto (solo si el campo está vacío) ----
        document.querySelectorAll("input[data-default]").forEach(function (el) {
            var vacio = !el.value || el.value.indexOf("0001-01-01") === 0;
            if (!vacio) return;
            el.value = el.dataset.default === "today" ? hoy : ahora;
        });

        // ---- 2. Fecha máxima = hoy (p. ej. incidentes no pueden ser futuros) ----
        document.querySelectorAll("input[data-max='today']").forEach(function (el) { el.max = hoy; });

        // ---- 3. Solo dígitos ----
        document.querySelectorAll("input[data-only='digits']").forEach(function (el) {
            el.addEventListener("input", function () {
                var limpio = el.value.replace(/\D/g, "");
                if (limpio !== el.value) el.value = limpio;
            });
        });

        // ---- 4. Quitar espacios sobrantes al salir del campo ----
        document.querySelectorAll("input[data-clean], textarea[data-clean]").forEach(function (el) {
            el.addEventListener("blur", function () {
                el.value = el.value.trim().replace(/\s+/g, " ");
            });
        });

        // ---- 5. Desplegables con búsqueda ----
        document.querySelectorAll("select.searchable").forEach(function (el) {
            if (typeof TomSelect === "undefined") return; // sin la librería queda un <select> normal
            new TomSelect(el, {
                create: false,
                maxOptions: null,
                allowEmptyOption: true,
                plugins: ["clear_button"],
                placeholder: el.dataset.placeholder || "Escriba para buscar…",
                render: { no_results: function () { return '<div class="no-results">Sin resultados</div>'; } }
            });
        });

        function limpiarSelect(sel) {
            if (!sel) return;
            if (sel.tomselect) sel.tomselect.clear(true);
            else sel.value = "";
        }

        // ---- 6. Select dependiente (dirección de entrega según el cliente) ----
        document.querySelectorAll("select[data-depends-on]").forEach(function (dep) {
            var padre = document.getElementById(dep.dataset.dependsOn);
            var fuente = window[dep.dataset.source] || {};
            if (!padre) return;

            padre.addEventListener("change", function () {
                var items = fuente[padre.value] || [];
                var ts = dep.tomselect;

                if (ts) {
                    ts.clear(true);
                    ts.clearOptions();
                    ts.addOptions(items);
                    ts.refreshOptions(false);
                    if (items.length === 1) ts.setValue(items[0].value);   // una sola: se elige sola
                } else {
                    dep.innerHTML = '<option value="">Seleccione…</option>';
                    items.forEach(function (it) {
                        var o = document.createElement("option");
                        o.value = it.value; o.textContent = it.text;
                        dep.appendChild(o);
                    });
                    if (items.length === 1) dep.value = items[0].value;
                }
            });
        });

        // ---- 7. Domiciliario -> completa / limpia fecha y hora de asignación ----
        document.querySelectorAll("select[data-assign-date]").forEach(function (sel) {
            var f = document.getElementById(sel.dataset.assignDate);
            var h = document.getElementById(sel.dataset.assignTime);
            sel.addEventListener("change", function () {
                if (sel.value) {
                    if (f && !f.value) f.value = hoy;
                    if (h && !h.value) h.value = ahora;
                } else {
                    if (f) f.value = "";
                    if (h) h.value = "";
                }
            });
        });

        // ---- 8. Si se elige uno, se limpia el otro (establecimiento <-> dirección externa) ----
        document.querySelectorAll("select[data-excludes]").forEach(function (sel) {
            var otro = document.getElementById(sel.dataset.excludes);
            sel.addEventListener("change", function () {
                if (sel.value) limpiarSelect(otro);
            });
        });

        // ---- 9. Secciones que dependen del tipo de usuario (data-show-for="domiciliario,ambos") ----
        var radiosTipo = document.querySelectorAll('input[name="Tipo"][type="radio"]');
        var seccionesTipo = document.querySelectorAll("[data-show-for]");

        function sincronizarRequeridos() {
            document.querySelectorAll("[data-req-when-shown]").forEach(function (el) {
                var cont = el.closest("[data-show-for]");
                el.required = !cont || !cont.hidden;
            });
        }
        function aplicarTipo() {
            var marcado = document.querySelector('input[name="Tipo"]:checked');
            var tipo = marcado ? marcado.value : "";
            seccionesTipo.forEach(function (sec) {
                sec.hidden = sec.dataset.showFor.split(",").indexOf(tipo) === -1;
            });
            sincronizarRequeridos();
        }
        radiosTipo.forEach(function (r) { r.addEventListener("change", aplicarTipo); });
        if (radiosTipo.length) aplicarTipo(); else sincronizarRequeridos();
    });
})();
