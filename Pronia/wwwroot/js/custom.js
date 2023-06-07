let basketBtn = document.querySelectorAll(".add-basket")
basketBtn.forEach((btn) => {
    btn.addEventListener("click", (e) => {
        e.preventDefault();
        let url = btn.getAttribute("href");
        fetch(url).then(response =>
            response.text())
            .then(data => {

                $("#miniCart .basket-model").html(data)

            }
            )

    })
})

$(document).on("click", ".removefrombasket", function (e) {
    e.preventDefault();
    let url = $(this).attr("href");
    fetch(url)
        .then(response => {
            if (!response.ok) {
                alert("xeta bas verdi")
                return
            }
            return response.text()
        })
        .then(data => {data.
            $("#miniCart .basket-model").html(data)
        })
})

