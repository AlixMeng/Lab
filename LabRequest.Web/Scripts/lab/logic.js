var getRequestTitles = $('#getRequestTitles').data('request-url');
var getUnitName = $('#getUnitName').data('request-url');
var getDetailsOfRequestTitle = $('#getDetailsOfRequestTitle').data('request-url');
var getAllRequestDetails = $('#getAllRequestDetails').data('request-url');
var ajaxloader = $('#ajax-loader').data('request-url');
var confirmreqtest = $('#ConfirmReqTests').data('request-url');


//function showDate(source) {
//    $(source).MdPersianDateTimePicker({
//        Placement: 'top',
//        Trigger: 'focus',
//        EnableTimePicker: false,
//        TargetSelector: '',
//        GroupId: '',
//        ToDate: false,
//        FromDate: false,
//        EnglishNumber: false
//    });
//}


function PersianCalender(source) {
    $(source).datepicker({
        format: "yyyy/mm/dd",
        minViewMode: 0,
        autoclose: true
    });
}


function fillDropDownLists(source, target, url, flag, rpt) {
    if (source.val() != '-1') {
        if (flag == true) {
            $("#RequestTitle").empty();
            $("#dvCheckBoxListControl").empty();
        };
        var id = source.val();
        $.getJSON(url, { id: id }, function (data) {
            target.empty();
            if (flag == true) {
                //$("#btnSave").attr("class", "btn btn-primary disabled");
            }
            else {
                //$("#btnSave").attr("class", "btn btn-primary");
            }
            $.each(data, function (index, items) {
                if (flag == true) {
                    target.append($('<option/>',
                        {
                            value: items.Value,
                            text: items.Text
                        }));
                }
                else {
                    target.append("<div class='col-lg-6'>" +
                        "<input type='checkbox' name='RequestDetail' value=" +
                        items.Value + "> <label>" + items.Text + "</label> </div>");
                }
            });
            if (flag == true && rpt == true) {
                $(target).prepend("<option value='' selected='selected'></option>");
            }
        });
    }
}