
var rateList = new Array();

$(document).ready(function () {
    $("#Amount").change(CalcSumAmount);
    $("#SumAmount").change(CalcAmount);
    $("#TaxationType").change(taxChange);

})

function TaxBind(id) {
    $.ajax({
        url: "/InternalAPI/GetTaxList?TaxPayer="+id,
        async: false,
        dataType: "json",
        success: function (d) {

            $('#TaxationType').multiselect(SelectConfigSetting);
            $("#TaxationType").multiselect('dataprovider', d);
            $("#TaxationType").val("").multiselect("refresh");
        }
    });

}

function CalcSumAmount() {
    //根据收入计算总金额
    var amount = $("#Amount").val().replace(/,/g, "");
    //千位符
    $("#Amount").val(Money(amount));

    var taxType = $("#TaxationType").val();
    if (taxType == "" || taxType == null) {
        $("#TaxationAmount").val(0);
        $("#SumAmount").val(Money(amount));
    }
    else {
        var rate;
        rate = GetTaxRate(taxType);

        var sumAmount = (amount + (amount*(rate / 100))).toFixed(2);
        $("#SumAmount").val(Money(sumAmount));
        var tax = Money(sumAmount - amount)
        $("#TaxationAmount").val(tax);

    }
}

function CalcAmount() {
    //根据收入计算总金额
    var sumAmount = $("#SumAmount").val().replace(/,/g, "");
    //千位符
    $("#SumAmount").val(Money(sumAmount));

    var taxType = $("#TaxationType").val();
    if (taxType == "" || taxType == null) {
        $("#TaxationAmount").val(0);
        $("#Amount").val(Money(sumAmount));
    }
    else {
        var rate;
        rate = GetTaxRate(taxType);
        var amount = (sumAmount / (1 + rate / 100)).toFixed(2);
        $("#Amount").val(Money(amount));
        var tax = Money(sumAmount - amount)
        $("#TaxationAmount").val(tax);

    }
}

function taxChange() {
    var taxType = $("#TaxationType").val();
    var sum = $("#SumAmount").val().replace(/,/g, "");
    var amount = $("#Amount").val().replace(/,/g, "");

    if (taxType == null) {
        $("#TaxationAmount").val(0)

        if (sum != "") {
            $("#Amount").val(sum)
        }
        else {
            $("#SumAmount").val(amount)
        }
        return false;
    }
    var tax = 0;
    if (sum != "") {
        CalcAmount();
        return false;
    }
    if (amount != "") {
        CalcSumAmount()
        return false;
    }
    return true;
}

function GetTaxRate(taxType) {
    var rate = rateList[taxType];

    if (rate == null) {
        rate = QueryTaxRate(taxType);
    }
    return rate;
}


function QueryTaxRate(taxType) {
    var rate = 0;
    $.ajax({
        url: "/InternalAPI/GetTaxOne",
        async: false,
        data: { value: taxType },
        dataType: "json",
        success: function (data) {
            rate = data.Rate
            rateList[taxType] = rate;
        }
    });
    return rate;
}

