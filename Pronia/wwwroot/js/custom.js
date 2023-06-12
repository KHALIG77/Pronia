let basketBtn = document.querySelectorAll(".add-basket")
/*let totalCount = document.querySelectorAll(".quantity")*/


basketBtn.forEach((btn) => {
    btn.addEventListener("click", (e) => {
        e.preventDefault();
        let url = btn.getAttribute("href");
        fetch(url).then(response =>
            response.text())
            .then(data => {

                $("#miniCart .basket-model").html(data)
                var totalCount = $("#allCount").val()
               $(".quantity").html(totalCount)
              
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
        .then(data => {
            $("#miniCart .basket-model").html(data)
            var totalCount = $("#allCount").val()
            $(".quantity").html(totalCount)
        })
})


