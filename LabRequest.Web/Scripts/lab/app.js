$(document).ready(function () {


//        var replaceDigits = function() {
//            var map =
//            [
//                "&\#1632;", "&\#1633;", "&\#1634;", "&\#1635;", "&\#1636;",
//                "&\#1637;", "&\#1638;", "&\#1639;", "&\#1640;", "&\#1641;"
//            ];
//            document.body.innerHTML =
//                document.body.innerHTML.replace(
//                    /\d(?=[^<>]*(<|$))/g,
//                    function($0) { return map[$0] }
//                );
//        };
//        Window.load = replaceDigits();


    $('ul.nav.navbar-nav').find('a[href="' + location.pathname + '"]')
        .closest('li').addClass('active');


    PersianCalender($('#RequestDate'));
    PersianCalender($('#ReportFromDate'));
    PersianCalender($('#ReportToDate'));
    PersianCalender($("#txtdate"));


    $(".validation-summary-errors").removeClass("validation-summary-errors");
    $(".input-validation-error").removeClass("input-validation-error")
        .parent().addClass("has-error");


    $("#Company").click(function() {
        fillDropDownLists($("#Company"), $("#Unit"), getUnitName, true, false);
    });
    $("#Company").change(function() {
        fillDropDownLists($("#Company"), $("#Unit"), getUnitName, true, false);
    });
    $("#Unit").change(function() {
        fillDropDownLists($("#Unit"), $("#RequestTitle"), getRequestTitles, true, false);
    });
    $("#RequestTitle").change(function() {
        fillDropDownLists($("#RequestTitle"), $("#dvCheckBoxListControl"),
            getDetailsOfRequestTitle, false, false);
    });


    $("#ReportCompany").change(function() {
        fillDropDownLists($("#ReportCompany"), $("#ReportUnit"),
            getUnitName, true, true);
    });
    $("#ReportUnit").change(function() {
        fillDropDownLists($("#ReportUnit"), $("#ReportRequestTitle"),
            getRequestTitles, true, true);
    });
    $("#ReportRequestTitle").change(function() {
        fillDropDownLists($("#ReportRequestTitle"), $("#ReportTestName"),
            getDetailsOfRequestTitle, true, true);
    });


    $(".btndetail").click(function () {
        var result = $("#result");
        result.empty();
        result.append("<p class='text-center'><img src='" + ajaxloader + "' /></p>");
        var testid = $(this).closest('tr').find('td:eq(0) input').val();
        var reqgenid = $(this).closest('tr').find('td:eq(1) input').val();
        $.getJSON(getAllRequestDetails, { id: testid, reqgenid: reqgenid },
            function(items) {
                result.empty();
                var tableresult = "<div class='panel panel-primary'><div class='panel-heading'>" +
                    "<i class='fa fa-paperclip'></i>&#160 مشخصات درخواست ثبت شده</div> " +
                    "<table id='tblresult' class='table table-striped'><thead><tr><th>نام تست</th> " +
                    "<th>تاریخ تایید</th><th>ساعت تایید</th><th>وضعیت</th></tr></thead><tbody>";
                $.each(items, function(index, itemData) {
                    var date = (itemData.date == null) ? 'ثبت نشده' : itemData.date;
                    var time = (itemData.time == null) ? 'ثبت نشده' : itemData.time;
                    tableresult += "<td>" + itemData.test
                        + "</td><td>" + date
                        + "</td><td>" + time + "</td><td>"
                        + itemData.confirm + "</td></tr>";
                });
                tableresult += "</tbody></table></div>";
                result.append(tableresult);
            });
    });
});