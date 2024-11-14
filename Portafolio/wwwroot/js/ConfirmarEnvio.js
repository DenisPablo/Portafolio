function confirmar()
{
    const confirmar = document.getElementById("confirmar");
    const guardar = document.getElementById("guardar");

    confirmar.style.display = "block";
    guardar.style.display = "none";
}


function borrarImagen(publicID) {
    const baseURL = `${window.location.origin}/Proyecto/BorrarImagen`

    fetch(baseURL,
        {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify("mgkzweanh8xdfwk0oljn")
        })
}