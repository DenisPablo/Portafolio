function filtrarProyectos() {
  let input = document.getElementById("buscador").value.toLowerCase();
  let proyectos = document.querySelectorAll(".proyecto-item");

  proyectos.forEach((proyecto) => {
    let titulo = proyecto.getAttribute("data-titulo");

    if (titulo.includes(input)) {
      proyecto.style.display = "block";
    } else {
      proyecto.style.display = "none";
    }
  });
}
