
$(document).ready(function () {


    $("#brokerId").on("change", function () {
        $("#reload").submit();
    });

    $("#shareId").on("change", function () {
        $("#reload").submit();
    });

    $("#amount").keyup(function () {
        if ($("#amount").val() != "") {
            if (parseInt($("#amount").val()) > parseInt($("#shareQuantity").val())) {
                $("#price").text("");
                $("#priceHidden").val("");
                $("#amount").attr("value", "0");
                $("#btnBuy").attr("disabled", "disabled");
                alert("Please Enter Valid Amount Of Shares");
            } else {
                var price = $("#amount").val() * $("#sharePrice").val();
                $("#price").text(price.toFixed(2));
                $("#priceHidden").val(price.toFixed(2));
                $("#btnBuy").removeAttr("disabled");
            }
        } else {
            $("#price").text("");
            $("#priceHidden").val("");
            $("#btnBuy").attr("disabled", "disabled");
        }
    });
});