function borrarImagen(publicID) {
    const baseURL = `${window.location.origin}/Proyecto/BorrarImagen`

    fetch(baseURL,
        {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(publicID)
        })
}