
var rateList = new Array();

$(document).ready(function () {
    $("#resaleModal #ResaleActualAmount").change(CalcSumAmount);//收入金额
    $("#resaleModal #SumAmounts").change(CalcAmount);//含税总收入
    $("#resaleModal #TaxationType").change(taxChange);//税种
    
})

function TaxBind() {
    $.ajax({
        url: "/InternalAPI/GetTaxList",
        async: false,
        dataType: "json",
        success: function (d) {
            $("#resaleModal #TaxationType").multiselect(SelectConfigSetting);
            $("#resaleModal #TaxationType").multiselect('dataprovider', d);
            $("#resaleModal #TaxationType").val("").multiselect("refresh");
        }
    });

}

function CalcSumAmount() {
    //根据收入计算总金额
    var amount = $("#resaleModal #ResaleActualAmount").val().replace(/,/g, "");
    //千位符
    $("#resaleModal #ResaleActualAmount").val(Money(amount));

    var taxType = $("#resaleModal #TaxationType").val();
    if (taxType == "" || taxType == null) {
        $("#resaleModal #TaxationAmounts").val(0);
        $("#resaleModal #SumAmounts").val(Money(amount));
    }
    else{
        var rate;
        rate = GetTaxRate(taxType);
        var sumAmount = (amount * (1 + rate / 100)).toFixed(2);
        $("#resaleModal #SumAmounts").val(Money(sumAmount));
        var tax = Money(sumAmount - amount)
        $("#resaleModal #TaxationAmounts").val(tax);

    }
}

function CalcAmount() {
    //根据收入计算总金额
    var sumAmount = $("#resaleModal #SumAmounts").val().replace(/,/g, "");
    //千位符
    $("#resaleModal #SumAmounts").val(Money(sumAmount));
    var taxType = $("#resaleModal #TaxationType").val();
    if (taxType == "" || taxType == null) {
        $("#resaleModal #TaxationAmounts").val(0);
        $("#resaleModal #ResaleActualAmount").val(Money(sumAmount));
    }
    else {
        var rate;
        rate = GetTaxRate(taxType);

        var amount = (sumAmount * (1 - rate / 100)).toFixed(2);
        $("#resaleModal #ResaleActualAmount").val(Money(amount));
        var tax = Money(sumAmount - amount)
        $("#resaleModal #TaxationAmounts").val(tax);

    }
}

function taxChange() {
    var taxType = $("#resaleModal #TaxationType").val();
    var sum = $("#resaleModal #SumAmounts").val().replace(/,/g, "");
    var amount = $("#resaleModal #ResaleActualAmount").val().replace(/,/g, "");
    if (taxType == null) {
        $("#resaleModal #TaxationAmounts").val(0)

        if (sum != "") {
            $("#resaleModal #ResaleActualAmount").val(sum)
        }
        else {
            $("#resaleModal #SumAmounts").val(amount)
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
//通过单价和数量计算转售金额
function ResaleNumberChange(ResaleNumber) {
    var ResaleNumber = ResaleNumber;
    var InventoryQuantity = $("#resaleModal #InventoryQuantity").val();
    if (parseInt(ResaleNumber) > parseInt(InventoryQuantity)) {
        alert("转售数量不得大于库存数量");
        $("#resaleModal #ResaleNumber").val("");
        error = true;
        return false;
    }
    var unitprice = $("#resaleModal #unitprice").val();
    var Amountinfos = ResaleNumber * unitprice;
    $("#resaleModal #Amountinfos").val(Amountinfos);  
}
function removeClass() {
    $(".filter").hide();

}



