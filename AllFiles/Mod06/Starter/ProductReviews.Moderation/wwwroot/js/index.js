const baseurl = "https://localhost:5001";
const $reviews = $("#reviews");


function reviewItem(review, type) {
    let $div = $("<div>").addClass("list-group-item flex-column align-items-start");
    $div.attr("id", review.id);
    $("<h2>").text(`Review ${type}`).appendTo($div);
    $("<h3>").text(`Author: ${review.author}`).appendTo($div);
    $("<h3>").text(`Email: ${review.email}`).appendTo($div);
    $("<h3>").text(`Score: ${review.score}`).appendTo($div);
    $("<h3>").text(`Text: ${review.text}`).appendTo($div);
    $("<button>").text("Accept")
        .addClass("btn btn-success")
        .click(evt => $div.remove())
        .appendTo($div);
    $("<button>").text("Remove")
        .addClass("btn btn-danger")
        .click(evt => {
            $.ajax({
                url: `${baseurl}/review/${review.id}`,
                method: "delete"
            }).done(() => $div.remove())
        })
        .appendTo($div);
    return $div;
}


