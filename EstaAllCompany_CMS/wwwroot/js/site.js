// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {

    if ($("a.confirmDeletion").length) {   //Bunun yaşam alanında
        $("a.confirmDeletion").click(() => {  //Bu tuş click oldugunda
            if (!confirm("Confirm deletetion")) return false;//eğer ekrana gelen mesaj yani confirm deletetetion yazsın eger evete basılırsa sil hayıra basılırsa silme demek için bu script ekstra önlem amaçlı yazılmıştır.
        });
    }
    //Yukarıdaki function <a asp-action="Delete" class="btn btn-danger confirmDeletion" asp-route-id="@item.Id">Delete</a> butonu Admin/Views/Page/Index içerindeki Delete butonu için yazılmıştır (a.confirmDeletion) = a içerisindeki confirmDeletion classının Row'unu yani Length o anlama geliyor burada sil diyorum bu script ile

    if ($("div.alert.notification").length) {
        setTimeout(() => {
            $("div.alert.notification").fadeOut();
        }, 1000);
        //Bu function sayesinde notificationın devreyi girip 2000 saniye sonra yok olsun diye dinamik bir şekilde oluşturmuş oluyorum.ÖNEMLİ div.alert.notification= Bu kısımı _NotificationPartildaki gibi div'in içindeki Alert classındaki notification metodunu al diyoruz.
        //Bu js'de zaten _Layoutumda mevcut geldim direk dinamik olsun diye buraya yazdım.
    }

});

function readURL(input) {
    if (input.files && input.files[0]) {
        let reader = new FileReader();

        reader.onload = function (e) {
            $("img#imgpreview").attr("src", e.target.result).width(200).height(200);
        };

        reader.readAsDataURL(input.files[0]);
    }
}
