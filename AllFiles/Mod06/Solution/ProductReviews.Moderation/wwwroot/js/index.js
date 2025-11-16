const baseurl = "https://localhost:5001";
const connection = new signalR.HubConnectionBuilder().withUrl(`${baseurl}/reviewmonitor`).build();
const $reviews = $("#reviews");

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});
connection.on("ReviewAdded", function (review) {
    var rev = reviewItem(review, "added")
    $reviews.append(rev);
});
connection.on("ReviewModified", function (review) {
    var rev = reviewItem(review, "modified")
    $reviews.append(rev);
});

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


