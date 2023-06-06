let quickBtn = document.querySelectorAll("#quick-view")

quickBtn.forEach((btn) => {
    btn.addEventListener("click",(e) => {
        e.preventDefault();
        let url = btn.getAttribute("href")
        fetch(url)
            .then(Response => Response.text())
            .then(data => {
                $("#quickModal .modal-dialog").html(data)
                $("#quickModal").modal("show")
            })
    })
})