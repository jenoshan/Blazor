function PrintDiv(Com_Nam, Addr_One, Cit, Contry_Nam, Post_Cod, Inv_No, Inv_Date, InvDue_Date, ToCom_Nam, ToContry_Nam, Tot_Amount, Re_Mark, Cur_Cod, Img_Logo, Bank_Nam, Acc_Nam, Branch, Acc_Num, invoiceline) {

    var contents1 = $("#printSummary1").html();
    // var res = contents.replaceAll("http://", "https://");
    //var res2 = res.replace("http", "https");
    //var res3 = res2.replace("http", "https");
    var frame1 = $('<iframe />');
    frame1[0].name = "frame1";
    frame1.css({ "position": "center", "center": "-1000000px" });
    $("body").append(frame1);
    var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
    frameDoc.document.open();
    //Create a new HTML document.
    frameDoc.document.write('<html><head><title>Invoice Report</title>');
    //Append the external CSS file.

    frameDoc.document.write('<link rel="stylesheet" href="css/bootstrap/bootstrap.min.css" />');



    frameDoc.document.write('</head><body>');
    frameDoc.document.write('<style>#info{#printArea:margin-top:10%; }  </style>');


    frameDoc.document.write('<div class="container">');

    frameDoc.document.write('<div class="col-12" style="background-color:#0F4C75;color:white;">');

    frameDoc.document.write('<div class="row" style="padding: 40px 0px 20px 70px;">');


    frameDoc.document.write('<div class="col-2">');
    frameDoc.document.write('<img src="/assets/images/' + Img_Logo + '" width="100" height="100" />');
    frameDoc.document.write('</div>');


    frameDoc.document.write('<div class="col-7">');
    frameDoc.document.write('<h1> Invoice</h1>');
    frameDoc.document.write('</div>');

    frameDoc.document.write('<div class="col-3">');
    frameDoc.document.write('<div>');
    frameDoc.document.write('<p style="margin-bottom: 1px;">' + Com_Nam + '</p>');
    frameDoc.document.write('<p style="margin-bottom: 1px;">' + Addr_One + '</p>');
    frameDoc.document.write('<p style="margin-bottom: 1px;">' + Cit + '</p>');
    frameDoc.document.write('<p style="margin-bottom: 1px;">' + Contry_Nam + '</p>');
    frameDoc.document.write('</div>');
    frameDoc.document.write('</div>');

    frameDoc.document.write('</div>');

    frameDoc.document.write('</div>');



    frameDoc.document.write('<div class="col-12" style="margin-bottom: -30px;">');
    frameDoc.document.write('<div class="row" style="padding: 40px 0px 20px 70px;">');

    frameDoc.document.write('<div class="col-7">');
    frameDoc.document.write('<div>');
    frameDoc.document.write('<p style="margin-bottom: 1px;">Bill For ' + ToCom_Nam + '</p>');
    frameDoc.document.write('<p style="margin-bottom: 1px;">' + ToContry_Nam + '</p>');

    frameDoc.document.write('</div>')
    frameDoc.document.write('</div>')



    frameDoc.document.write('<div class="col-5">');
    frameDoc.document.write('<div>');

    frameDoc.document.write('<div class="row"style="margin-bottom: -15px;">');
    frameDoc.document.write('<div class="col-6">');
    frameDoc.document.write('<strong>Invoice NO</strong>');
    frameDoc.document.write('</div>')

    frameDoc.document.write('<div class="col-6">');
    frameDoc.document.write('<p>#' + Inv_No + '</p>');
    frameDoc.document.write('</div>')
    frameDoc.document.write('</div>')

    frameDoc.document.write('<div class="row"style="margin-bottom: -15px;">');
    frameDoc.document.write('<div class="col-6">');
    frameDoc.document.write('<strong>Invoice Date</strong>');
    frameDoc.document.write('</div>')

    frameDoc.document.write('<div class="col-6">');
    frameDoc.document.write('<p>' + Inv_Date + '</p>');
    frameDoc.document.write('</div>')
    frameDoc.document.write('</div>')





    frameDoc.document.write('<div class="row"style="margin-bottom: -15px;">');
    frameDoc.document.write('<div class="col-6">');
    frameDoc.document.write('<strong>Invoice Due Date</strong>');
    frameDoc.document.write('</div>')

    frameDoc.document.write('<div class="col-6">');
    frameDoc.document.write('<p>' + InvDue_Date + '</p>');
    frameDoc.document.write('</div>')
    frameDoc.document.write('</div>')


    frameDoc.document.write('<div class="row"style="margin-bottom: -15px;">');
    frameDoc.document.write('<div class="col-6">');
    frameDoc.document.write('<strong>Total Amount</strong>');
    frameDoc.document.write('</div>')

    frameDoc.document.write('<div class="col-6">');
    frameDoc.document.write('<p>' + Cur_Cod + '  ' + Tot_Amount + '</p>');
    frameDoc.document.write('</div>')
    frameDoc.document.write('</div>')

    frameDoc.document.write('</div>')
    frameDoc.document.write('</div>')



    frameDoc.document.write('</div>')
    frameDoc.document.write('</div>')




    frameDoc.document.write('<div class="col-md-12" style="padding: 40px 0px 20px 70px;">');
    frameDoc.document.write('<table class="table text-white">');

    var a;
    for (a = 0; i < invoiceline.length; a++) {
    }

    var sum = 0;
    for (var i = 0; i < invoiceline.length; i++) {
        sum += invoiceline[i].amount;
    }

    frameDoc.document.write('<thead>');
    frameDoc.document.write('<tr>');
    frameDoc.document.write('<th class="borderremover">Description</th>');
    if (invoiceline[a].qty != null) {
        frameDoc.document.write('<th class="borderremover">Quentity</th>');
    }
    frameDoc.document.write('<th class="borderremover">Amount</th>');
    frameDoc.document.write('</tr>');
    frameDoc.document.write('</div>')


    frameDoc.document.write('<tbody>')
    for (var i = 0; i < invoiceline.length; i++) {

        frameDoc.document.write('<tr>');
        frameDoc.document.write('<td class="borderremover">' + invoiceline[i].description + '</td>');
        if (invoiceline[i].qty != null) {
            frameDoc.document.write('<td class="borderremover">' + invoiceline[i].qty + '</td>');
        } else {
            frameDoc.document.write('<td class="borderremover"></td>');
        }
        var totamount = parseInt(invoiceline[i].amount);
        let Tot_Amount = totamount.toFixed(2)
     

        frameDoc.document.write('<td class="borderremover">' + Tot_Amount+ '</td>');
        frameDoc.document.write('</tr>');
    }
    frameDoc.document.write('</tbody>')


    frameDoc.document.write('</table>');
    frameDoc.document.write('</div>');



    frameDoc.document.write('<div class="col-md-12" style="background-color: #0F4C75;color:white;">');
    frameDoc.document.write('<div class="row" style="padding: 30px 0px 20px 50px;">');

    frameDoc.document.write('<div class="col-9">');
    frameDoc.document.write('<div>');
    if (Bank_Nam == null && Acc_Nam == null && Branch == null) {
        frameDoc.document.write(' <p style="font-size:20px;" class="text-justify"> <strong>Bank Details : </strong></p >');
    } else {
        frameDoc.document.write(' <p style="font-size:20px;" class="text-justify"> <strong>Bank Details : </strong>' + Bank_Nam + '<br />' + Acc_Nam + '&nbsp;' + Branch + '</p >');

    }

    frameDoc.document.write('</div>');
    frameDoc.document.write('</div>');

    frameDoc.document.write('<div class="col-3">');
    frameDoc.document.write('<div>');
    frameDoc.document.write('<strong>Account Number</strong> ');
    if (Acc_Num == null) {
        frameDoc.document.write('<h2></h2>');
    } else {
        frameDoc.document.write('<h2>' + Acc_Num + '</h2> ');
    }

    frameDoc.document.write('</div>');
    frameDoc.document.write('</div>');

    frameDoc.document.write('</div>');
    frameDoc.document.write('</div>');

    frameDoc.document.write('<br />');

    frameDoc.document.write('<div class="col-md-12" style="background-color: #0F4C75;color:white;">');
    frameDoc.document.write('<div class="row" style="padding: 30px 0px 20px 50px;">');

    frameDoc.document.write('<div class="col-9">');
    frameDoc.document.write('<div>');
    frameDoc.document.write('<strong style="font-size:20px;">' + Re_Mark + '</strong>');
    frameDoc.document.write('</div>');
    frameDoc.document.write('</div>');

    frameDoc.document.write('<div class="col-3">');
    frameDoc.document.write('<div>');
    frameDoc.document.write('<strong>Total</strong> ');
    frameDoc.document.write('<h2>' + Cur_Cod + ' ' + Tot_Amount + '</h2> ');
    frameDoc.document.write('</div>');
    frameDoc.document.write('</div>');

    frameDoc.document.write('</div>');
    frameDoc.document.write('</div>');

    frameDoc.document.write('</div>');

    frameDoc.document.write('</body></html>');
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["frame1"].focus();
        window.frames["frame1"].print();
        frame1.remove();
    }, 5000);
}

