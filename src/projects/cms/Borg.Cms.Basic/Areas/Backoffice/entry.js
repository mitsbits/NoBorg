require('admin-lte');
require('jquery');
require('bootstrap')

jQuery.expr[':'].icontains = function (a, i, m) {
    return jQuery(a).text().toUpperCase()
        .indexOf(m[3].toUpperCase()) >= 0;
};
$(function () {


    $(".server-table").dataTable({
        "bPaginate": false,
        "bLengthChange": false,
        "bFilter": false,
        "bSort": false,
        "bInfo": false,
        "bAutoWidth": true
    });

    $('input.datetimepicker').daterangepicker({
        "timePicker": true, "timePickerIncrement": 15, "format": 'DD/MM/YYYY HH:mm', "timePicker24Hour": true,
        "singleDatePicker": true//, startDate: moment().subtract(29, 'days'),
    });
    $("input[type='checkbox']:not(.simple), input[type='radio']:not(.simple)").iCheck(
        {
            checkboxClass: 'icheckbox_minimal-grey',
            radioClass: 'iradio_minimal-grey'
        }
    );
    $("select.select2").select2();

    //$('input.datetimepicker').daterangepicker({
    //    "timePicker": true, "timePickerIncrement": 5, "format": 'DD/MM/YYYY HH:mm', "timePicker24Hour": true,
    //    "singleDatePicker": true,//, startDate: moment().subtract(29, 'days'),
    //});
    $('input.datepickernullable').daterangepicker({
        "singleDatePicker": true, locale: {
            format: 'DD/MM/YYYY'
        },
    });

    //$('.pull-down').each(function () {
    //    var $this = $(this);
    //    $this.css('margin-top', $this.parent().height() - $this.height());
    //});

    //var serverdatetime = new Date($('input#loadclock').val());
    //startTime();

    //function startTime() {
    //    var today = serverdatetime;
    //    var h = today.getHours();
    //    var m = today.getMinutes();
    //    var s = today.getSeconds();

    //    m = checkTime(m);
    //    s = checkTime(s);
    //    $('span#clock').html(h + ":" + m + ":" + s);
    //    serverdatetime = new Date(serverdatetime.getTime() + 1000);
    //    var t = setTimeout(startTime, 1000);
    //}
    //function checkTime(i) {
    //    if (i < 10) { i = "0" + i };  // add zero in front of numbers < 10
    //    return i;
    //}
    var menusnapshot = $("ul.sidebar-menu").html();
    $("#menu-search").submit(function (event) {
        var q = $("#q").val();
        if (q !== "undefined" && q.length > 0) {
            $("ul.sidebar-menu").hide();
            $("ul.sidebar-menu").html(menusnapshot);
            var lis = $("ul.sidebar-menu a:icontains('" + q + "')").closest("li");
            $("ul.sidebar-menu").empty();
            lis.hide();
            $("ul.sidebar-menu").append(lis);
            $("ul.sidebar-menu").show();
            $("ul.sidebar-menu li").fadeIn("normal");
        } else {
            $("ul.sidebar-menu").html(menusnapshot);
        }
        event.preventDefault();
    });
});