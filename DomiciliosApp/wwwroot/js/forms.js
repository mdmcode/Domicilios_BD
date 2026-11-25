/* forms.js — comportamiento de los formularios (Crear / Editar)
   - desplegables con búsqueda (Tom Select) en TODOS los select.form-select
   - filtros que se envían solos (data-auto-submit="true")
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
        // Un select NUNCA debe inicializarse dos veces (Tom Select lanza un error y aborta todo
        // lo que viene después), por eso se revisa "el.tomselect" antes de crear la instancia.
        function crearTomSelect(el) {
            if (typeof TomSelect === "undefined") return; // sin la librería queda un <select> normal
            if (el.tomselect) return;

            var autoEnvio = el.dataset.autoSubmit === "true";   // filtros que envían el formulario al cambiar

            new TomSelect(el, {
                create: false,
                maxOptions: null,
                allowEmptyOption: true,
                plugins: el.classList.contains("searchable") ? ["clear_button"] : [],
                placeholder: el.dataset.placeholder || "Escriba para buscar…",
                render: { no_results: function () { return '<div class="no-results">Sin resultados</div>'; } },
                onChange: function () {
                    if (autoEnvio && el.form) el.form.submit();
                }
            });
        }
        document.querySelectorAll("select.form-select, select.searchable").forEach(crearTomSelect);

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
            var aviso = dep.dataset.emptyHint ? document.getElementById(dep.dataset.emptyHint) : null;
            var avisoError = dep.dataset.errorHint ? document.getElementById(dep.dataset.errorHint) : null;
            var turno = 0;              // descarta respuestas viejas si el usuario cambia de cliente rápido
            var ultimo = padre.value;   // último cliente procesado (evita repetir la carga)

            // Si el select trae data-url, las direcciones se piden al servidor en cada cambio de cliente;
            // si no, se usa el diccionario que la vista dejó en la página (window[data-source]).
            function actualizar() {
                if (padre.value === ultimo) return;
                ultimo = padre.value;

                var miTurno = ++turno;
                if (avisoError) avisoError.hidden = true;

                if (!dep.dataset.url) { pintar(fuente[padre.value] || []); return; }
                if (!padre.value) { pintar([]); return; }

                fetch(dep.dataset.url + "?idCliente=" + encodeURIComponent(padre.value), { headers: { "Accept": "application/json" } })
                    .then(function (r) {
                        if (!r.ok) throw new Error("HTTP " + r.status);
                        return r.json();
                    })
                    .then(function (items) { if (miTurno === turno) pintar(items); })
                    .catch(function (err) {
                        console.error("No se pudieron cargar las direcciones del cliente:", err);
                        if (miTurno !== turno) return;
                        pintar([]);
                        if (avisoError) { avisoError.hidden = false; if (aviso) aviso.hidden = true; }
                    });
            }

            // Reconstruye el <select> desde cero y vuelve a crear el desplegable con búsqueda.
            function pintar(items) {
                // Cliente elegido pero sin direcciones registradas: se avisa en vez de dejar la lista vacía sin explicación
                if (aviso) aviso.hidden = !(padre.value && items.length === 0);

                if (dep.tomselect) dep.tomselect.destroy();

                dep.innerHTML = "";
                var vacio = document.createElement("option");
                vacio.value = "";
                vacio.textContent = "Seleccione…";
                dep.appendChild(vacio);

                items.forEach(function (it) {
                    var o = document.createElement("option");
                    o.value = String(it.value);
                    o.textContent = it.text;
                    dep.appendChild(o);
                });

                // una sola dirección: se elige sola
                dep.value = items.length === 1 ? String(items[0].value) : "";

                crearTomSelect(dep);
            }

            padre.addEventListener("change", actualizar);
            // Por si el desplegable con búsqueda del cliente no dispara el evento nativo
            if (padre.tomselect) padre.tomselect.on("change", actualizar);
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