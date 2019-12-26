$.ajaxSetup({ cache: false });
$customSearchDialog = "";
$customerSearchDialog = "";




function SuccessAlert(msg, duration) {
    if (msg != "") {
        duration = duration || 0;
        var dlg = document.createElement("div");
        dlg.setAttribute("id", "dialog_alert");
        dlg.setAttribute("class", "dialog_success");
        dlg.setAttribute("style", "z-index:100000;");

        dlg.innerHTML = "<span style='font-weight:bold; padding-bottom:15px;' >Success<" + "/span>";
        dlg.innerHTML = dlg.innerHTML + "<span class='closeBox' id='closeAlert' style='font-weight:bold; float:right;'>&#x0058;<" + "/span>";
        dlg.innerHTML = dlg.innerHTML + "<hrLineScc>";
        dlg.innerHTML = dlg.innerHTML + "<div style='font-weight:regular;  position:relative; padding: 10px 2px 10px 2px; overflow:auto; text-align:center;' >" + msg + "<" + "/div>"

        if (duration != 0) {
            setTimeout(function () {
                dlg.parentNode.removeChild(dlg);
            }, duration);
        }
        document.body.appendChild(dlg);
        $('.dialog_success').hide().fadeIn(500);
    }
}

function ErrorAlert(msg, duration) {
    debugger;
    if (msg != "") {
        duration = duration || 0;

        var dlg = document.createElement("div");
        dlg.setAttribute("id", "dialog_alert");
        dlg.setAttribute("class", "dialog_error");
        dlg.setAttribute("style", "z-index:100000;");

        dlg.innerHTML = "<span style='font-weight:bold; padding-bottom:15px;' >Error<" + "/span>";
        dlg.innerHTML = dlg.innerHTML + "<span class='closeBox' id='closeAlert' style='font-weight:bold; float:right;'>&#x0058;<" + "/span>";
        dlg.innerHTML = dlg.innerHTML + "<hrLineErr>";
        dlg.innerHTML = dlg.innerHTML + "<div style='font-weight:regular;  position:relative; padding: 10px 2px 10px 2px; overflow:auto; text-align:center;' >" + msg + "<" + "/div>"

        if (duration != 0) {
            setTimeout(function () {
                dlg.parentNode.removeChild(dlg);
            }, duration);
        }
        document.body.appendChild(dlg);
        $('.dialog_error').hide().fadeIn(500);
    }
}

function InfoAlert(msg, duration) {
    if (msg != "") {
        duration = duration || 0;

        var dlg = document.createElement("div");
        dlg.setAttribute("id", "dialog_alert");
        dlg.setAttribute("class", "dialog_info");
        dlg.setAttribute("style", "z-index:100000;");

        dlg.innerHTML = "<span style='font-weight:bold; padding-bottom:15px;' >Information<" + "/span>";
        dlg.innerHTML = dlg.innerHTML + "<span class='closeBox' id='closeAlert' style='font-weight:bold; float:right;'>&#x0058;<" + "/span>";
        dlg.innerHTML = dlg.innerHTML + "<hrLineInfo>";
        dlg.innerHTML = dlg.innerHTML + "<div style='font-weight:regular;  position:relative; padding: 10px 2px 10px 2px; overflow:auto; text-align:center;' >" + msg + "<" + "/div>"

        if (duration != 0) {
            setTimeout(function () {
                dlg.parentNode.removeChild(dlg);
            }, duration);
        }
        document.body.appendChild(dlg);
        $('.dialog_error').hide().fadeIn(500);
    }
}

function ShowCustomerDialog(html, title, position, duration) {
    try {
        position = position || "61px,0px,0px,205px";
        duration = duration || 0;

        var id = "customerDialog";

        var pos = position.split(",");
        var top = pos[0];
        var right = pos[1];
        var bottom = pos[2];
        var left = pos[3];

        var tag = '0,' + position;

        var cls = '.customDialog#' + id;

        $customerSearchDialog = document.createElement("div");
        $customerSearchDialog.setAttribute("class", "customDialog");
        $customerSearchDialog.setAttribute("id", id);
        $customerSearchDialog.setAttribute("style", "z-index:2;");

        var overlay = document.createElement("div");
        overlay.setAttribute("id", "dialogoverlay");
        overlay.setAttribute("style", "position:fixed;");

        var dlg = document.createElement("div");
        dlg.setAttribute("id", "dialog_Modal");
        dlg.setAttribute("class", "dialog_main");
        dlg.setAttribute("tag", tag);

        dlg.setAttribute("style", "left:" + left + ";right:" + right + ";top:" + top + ";bottom:" + bottom + "; min-width:300px;z-index:2;");
        dlg.innerHTML = dlg.innerHTML + "<span style='font-weight:bold; padding-bottom:15px;' >" + title + "<" + "/span>";

        dlg.innerHTML = dlg.innerHTML + "<span class='closeBox' id = 'closeCustomerDialog' style='font-weight:bold; float:right;'>&#x0058;<" + "/span>";
        dlg.innerHTML = dlg.innerHTML + "<span class='restoreBox' id='restoreDialogModal' style='font-weight:bold; float:right;'>&#9633;<" + "/span>";

        dlg.innerHTML = dlg.innerHTML + "<hrLineCalendar>";
        dlg.innerHTML = dlg.innerHTML + "<div class='dialog_container' id='divModal' >" + html + "<" + "/div>"

        $customerSearchDialog.appendChild(overlay);
        $customerSearchDialog.appendChild(dlg);

        if (duration != 0) {
            setTimeout(function () {
                var node = document.getElementById(id);
                while (node.hasChildNodes()) {
                    node.removeChild(node.firstChild);
                }
                $customerSearchDialog.parentNode.removeChild($customerSearchDialog);
            }, duration);
        }

//        document.body.appendChild($customerSearchDialog);
        
        

        $('#info_boxbody').append($customerSearchDialog);

        $(cls).hide().fadeIn(100);

        $(cls).find('input[type=text],textarea,select').filter(':visible:first').focus();

        //        $(cls).find(".dialog_container#divModal").mCustomScrollbar({
        //            scrollButtons: {
        //                enable: false
        //            },
        //            advanced: {
        //                updateOnContentResize: true
        //            }
        //        });
    } catch (err) {
        ErrorAlert(err, 5000);
    }
}

function CloseCustomerSearchDialog() {
    if ($customerSearchDialog != "") {
        $customerSearchDialog.parentNode.removeChild($customerSearchDialog);
        $customerSearchDialog = "";
    }
}


function ShowSearchBox(html, title, position, duration, id) {
    try {
        position = position || "61px,0px,0px,205px";
        duration = duration || 0;
        id = id || "";

        var pos = position.split(",");
        var top = pos[0];
        var right = pos[1];
        var bottom = pos[2];
        var left = pos[3];
        var tag = '0,' + position;


        if (id == "") {
            id = 'dialog_Search00' + $('.dialog_search').length + 1;
        }

        var cls = '.dialog_search#' + id;

        $customSearchDialog = document.createElement("div");
        $customSearchDialog.setAttribute("class", "dialog_search");
        $customSearchDialog.setAttribute("id", id);

        $customSearchDialog.setAttribute("tag", tag);

        $customSearchDialog.setAttribute("style", "left:" + left + ";right:" + right + ";top:" + top + ";bottom:" + bottom + ";z-index:5;");

        $customSearchDialog.innerHTML = $customSearchDialog.innerHTML + "<span style='font-weight:bold; padding-bottom:15px;' >" + title + "<" + "/span>";

        $customSearchDialog.innerHTML = $customSearchDialog.innerHTML + "<span class='closeBox' id = 'closeCustomSearch' style='font-weight:bold; float:right;'>&#x0058;<" + "/span>";
        $customSearchDialog.innerHTML = $customSearchDialog.innerHTML + "<span class='restoreBox' id='restoreCustomSearch' style='font-weight:bold; float:right;'>&#9633;<" + "/span>";

        $customSearchDialog.innerHTML = $customSearchDialog.innerHTML + "<hrLineCalendar>";
        $customSearchDialog.innerHTML = $customSearchDialog.innerHTML + "<div class='dialog_container' id='divNormal'  style='overflow:auto; padding:0px;'>" + html + "<" + "/div>"

        if (duration != 0) {
            setTimeout(function () {
                $customSearchDialog.parentNode.removeChild($customSearchDialog);
            }, duration);
        }
        $("#info_boxbody").append($customSearchDialog);

        $(cls).hide().fadeIn(500);
        $(cls).find('input[type=text],textarea,select').filter(':visible:first').focus();
    } catch (err) {
        ErrorAlert(err, 2000);
    }
}

function CloseCustomSearchDialog() {
    $customSearchDialog.parentNode.removeChild($customSearchDialog);
    $customSearchDialog = "";
}


function ShowMessageBox(html, title, param, id, updateinto, callfunction) {
    try {
        id = id || "Yes";
        callfunction = callfunction || "";

        var cls = '.customMsg#customMsg';
        var top = (window.innerHeight / 2) - 100;

        var main = document.createElement("div");
        main.setAttribute("class", "customMsg");
        main.setAttribute("id", "customMsg");
        main.setAttribute("param", param);
        main.setAttribute("updateinto", updateinto);
        main.setAttribute("callfunction", callfunction);

        var overlay = document.createElement("div");
        overlay.setAttribute("id", "dialogoverlay");
        overlay.setAttribute("style", "z-index:1999; opacity:0.1; background-color:Navy;")

        delDialog = document.createElement("div");

        delDialog.setAttribute("id", "dialog_msg");
        delDialog.setAttribute("class", "dialog_msg");

        delDialog.setAttribute("style", "top:" + top + "px;");
        delDialog.innerHTML = delDialog.innerHTML + "<span style='font-weight:bold; padding-bottom:15px;'>&nbsp;&nbsp;&nbsp;" + title + "<" + "/span>";

        delDialog.innerHTML = delDialog.innerHTML + "<span class='closeBox' id = 'closeMessage' style='font-weight:bold; margin: 1px 5px 0px 0px;'>&nbsp;&#x0058;<" + "/span>";


        delDialog.innerHTML = delDialog.innerHTML + "<hrLineCalendar>";


        var delMsg = document.createElement("div");
        delMsg.setAttribute("class", "msgbox_container");
        delMsg.setAttribute("style", "background-color:#DBE9DB; text-align:center; padding:10px 1px 10px 1px; margin:5px 5px 0px 5px;font-size:15px;border-radius:8px; ");
        delMsg.innerHTML = html;
        delDialog.appendChild(delMsg);

        var yesNo = document.createElement("div");
        yesNo.setAttribute("style", "width:100%; height:30px;");
        yesNo.innerHTML = "<input type= 'button' class='messageBox' id ='" + id + "' value='Yes' style='width:100px; float:left;margin-left:50px;'/>"
        yesNo.innerHTML += "<input type= 'button' id ='msgbox_Cancel' value='No' style='width:100px; float:right;margin-right:50px;'/>"

        delDialog.appendChild(yesNo);
        main.appendChild(overlay);
        main.appendChild(delDialog);

        document.body.appendChild(main);

        $(cls).hide().fadeIn(300);

        $(cls).find('input[type=text],input[type=button],textarea,select').filter(':visible:last').focus();

    } catch (err) {
        ErrorAlert(err, 5000);
    }
}

function CloseMe(id) {

    $(id).closest(".customDialog").remove();
}

function ShowDialogMenuTemp(html, title, position, id, duration) {
    try {

        position = position || "61px,0px,0px,205px";
        id = id || "";
        duration = duration || 0;

        var pos = position.split(",");
        var top = pos[0];
        var right = pos[1];
        var bottom = pos[2];
        var left = pos[3];

        var tag = '0,' + position;

        if (id == "") {
            id = 'customDialog00' + $('.customDialog').length + 1;
        }

        var cls = '.customDialog#' + id;

        var main = document.createElement("div");
        main.setAttribute("class", "customDialog");
        main.setAttribute("id", id);

        var overlay = document.createElement("div");
        overlay.setAttribute("id", "dialogoverlay");

        var dlg = document.createElement("div");
        dlg.setAttribute("id", "dialog_Modal");
        dlg.setAttribute("class", "dialog_main");
        dlg.setAttribute("tag", tag);

        dlg.setAttribute("style", "left:" + left + ";right:" + right + ";top:" + top + ";bottom:" + bottom + "; min-width:300px;");
        dlg.innerHTML = dlg.innerHTML + "<span style='font-weight:bold; padding-bottom:15px;' >" + title + "<" + "/span>";

        dlg.innerHTML = dlg.innerHTML + "<span class='closeBox' id = 'closeDialog' style='font-weight:bold; float:right;'>&#x0058;<" + "/span>";
        dlg.innerHTML = dlg.innerHTML + "<span class='restoreBox' id='restoreDialogModal' style='font-weight:bold; float:right;'>&#9633;<" + "/span>";

        dlg.innerHTML = dlg.innerHTML + "<hrLineCalendar>";
        dlg.innerHTML = dlg.innerHTML + "<div class='dialog_container' id='divModal' >" + html + "<" + "/div>"

        main.appendChild(overlay);
        main.appendChild(dlg);
        if (duration != 0) {
            setTimeout(function () {
                var node = document.getElementById(id);
                while (node.hasChildNodes()) {
                    node.removeChild(node.firstChild);
                }
                main.parentNode.removeChild(main);
            }, duration);
        }

        document.body.appendChild(main);

        $(cls).hide().fadeIn(500);

        $(cls).find(".dialog_container#divModal").mCustomScrollbar({
            scrollButtons: {
                enable: false
            },
            advanced: {
                updateOnContentResize: true
            }
        });

        $(cls).find('input[type=text],textarea,select').filter(':visible:first').focus();
    } catch (err) {
        ErrorAlert(err, 5000);
    }
}


function ShowDialogModal(html, title, position, id, duration) { 
    try {
        position = position || "A,61px,0px,0px,205px";
//     position = position || "R,20%,90px,50%,405px";
        id = id || "";
        duration = duration || 0;

       

        var VType="A";        
        var top="";
        var right="";
        var bottom="";
        var left="";
        var width="";
        var maxHeight = "";

        var pos = position.split(","); 

        
        if (pos.length == 4) {
            VType = "A";
            top = pos[0];
            right = pos[1];
            bottom = pos[2];
            left = pos[3];
        }
        else if (pos.length == 5) 
        {
            VType = pos[0];
            if (VType == "A") {
                top = pos[1];
                right = pos[2];
                bottom = pos[3];
                left = pos[4];
            }
            else {
                left = pos[1];
                top = pos[2];
                width = pos[3];
                maxHeight = pos[4];        
            }
        }

       
        var tag;
        if (pos.length == 5) {
            tag = '0,' + position;
        }
        else {
            tag = '0,A,' + position;
        }
        if (id == "") {
            id = 'customDialog00' + $('.customDialog').length + 1;
        }

        var cls = '.customDialog#' + id;

        var main = document.createElement("div");
        main.setAttribute("class", "customDialog");
        main.setAttribute("id", id);
        main.setAttribute("style", "display:none;z-index:2");

        var overlay = document.createElement("div");
        overlay.setAttribute("id", "dialogoverlay");
        overlay.setAttribute("style", "position:fixed;");

        var dlg = document.createElement("div");
        dlg.setAttribute("id", "dialog_Modal");
        dlg.setAttribute("class", "dialog_main");
        dlg.setAttribute("tag", tag);


        if (VType == "A") {
            dlg.setAttribute("style", "min-width:300px; left:" + left + ";right:" + right + ";top:" + top + ";bottom:" + bottom + "; z-index:2;");
        }
        else {
            dlg.setAttribute("style", "position:relative; padding:1px; left:" + left + ";width:" + width + ";top:" + top + "; min-width:300px;z-index:2;");
        }
        dlg.innerHTML = "<span style='font-weight:bold; padding-bottom:15px;' >" + title + "<" + "/span>";

        dlg.innerHTML = dlg.innerHTML + "<span class='closeBox' id = 'closeDialog' style='font-weight:bold; float:right;'>&#x0058;<" + "/span>";
        dlg.innerHTML = dlg.innerHTML + "<span class='restoreBox' id='restoreDialogModal' style='font-weight:bold; float:right;'>&#9633;<" + "/span>";

        dlg.innerHTML = dlg.innerHTML + "<hrLineCalendar>";
        if (VType == "A") {
            dlg.innerHTML = dlg.innerHTML + "<div class='dialog_container' id='divModal' >" + html + "<" + "/div>"
        }
        else {
            dlg.innerHTML = dlg.innerHTML + "<div class='dialog_container' id='divModal' style='bottom:auto; position:relative; margin:0px 2px 2px 0px; max-height:" + maxHeight + ";' >" + html + "<" + "/div>"
        }

        main.appendChild(overlay);
        main.appendChild(dlg);

        if (duration != 0) {
            setTimeout(function () {
                var node = document.getElementById(id);
                while (node.hasChildNodes()) {
                    node.removeChild(node.firstChild);
                }
                main.parentNode.removeChild(main);
            }, duration);
        }

        $('#info_boxbody').append(main);

        $(cls).fadeIn(500);
        $(cls).find('input[type=text],textarea,select').filter(':visible:first').focus();

    } catch (err) {
        ErrorAlert(err, 5000);
    }
}


function ShowDialogNormal(html, title, position, id, duration) {
    try {

        position = position || "A,61px,0px,0px,205px";
        //        position = position || "R,20%,90px,50%,405px";
        id = id || "";
        duration = duration || 0;

        var VType = "A";
        var top = "";
        var right = "";
        var bottom = "";
        var left = "";
        var width = "";
        var maxHeight = "";

        var pos = position.split(",");

        if (pos.length == 4) {
            VType = "A";
            top = pos[0];
            right = pos[1];
            bottom = pos[2];
            left = pos[3];
        }
        else if (pos.length == 5) {
            VType = pos[0];
            if (VType == "A") {
                top = pos[1];
                right = pos[2];
                bottom = pos[3];
                left = pos[4];
            }
            else {
                left = pos[1];
                top = pos[2];
                width = pos[3];
                maxHeight = pos[4];
            }
        }


        var tag;
        if (pos.length == 5) {
            tag = '0,' + position;
        }
        else {
            tag = '0,A,' + position;
        }

        if (id == "") {
            id = 'dialog_Normal00' + $('.customDialog').length + 1;
        }

        var cls = '.customDialog#' + id;

        var main = document.createElement("div");
        main.setAttribute("class", "customDialog");
        main.setAttribute("id", id);


        var dlg = document.createElement("div");
        dlg.setAttribute("class", "dialog_main");
        dlg.setAttribute("id", id);

        dlg.setAttribute("tag", tag);

//        dlg.setAttribute("style", "left:" + left + ";right:" + right + ";top:" + top + ";bottom:" + bottom + ";");

        if (VType == "A") {
            dlg.setAttribute("style", "left:" + left + ";right:" + right + ";top:" + top + ";bottom:" + bottom + "; min-width:300px;z-index:2;");
        }
        else {
            dlg.setAttribute("style", "position:relative; padding:1px; left:" + left + ";width:" + width + ";top:" + top + "; min-width:300px;z-index:2;");
        }



        dlg.innerHTML = dlg.innerHTML + "<span style='font-weight:bold; padding-bottom:15px;' >" + title + "<" + "/span>";

        dlg.innerHTML = dlg.innerHTML + "<span class='closeBox' id = 'closeNormal' style='font-weight:bold; float:right;'>&#x0058;<" + "/span>";
        dlg.innerHTML = dlg.innerHTML + "<span class='restoreBox' id ='restoreDialogNormal' style='font-weight:bold; float:right;'>&#9633;<" + "/span>";

        dlg.innerHTML = dlg.innerHTML + "<hrLineCalendar>";

        if (VType == "A") {
            dlg.innerHTML = dlg.innerHTML + "<div class='dialog_container' id='divModal' >" + html + "<" + "/div>"
        }
        else {
            dlg.innerHTML = dlg.innerHTML + "<div class='dialog_container' id='divModal' style='bottom:auto; position:relative; margin:0px 2px 2px 0px; max-height:" + maxHeight + ";' >" + html + "<" + "/div>"
        }

//        dlg.innerHTML = dlg.innerHTML + "<div class='dialog_container' id='divNormal'  style='overflow:auto;'>" + html + "<" + "/div>"


        main.appendChild(dlg);
        if (duration != 0) {
            setTimeout(function () {
                var node = document.getElementById(id);
                while (node.hasChildNodes()) {
                    node.removeChild(node.firstChild);
                }
                main.parentNode.removeChild(main);
            }, duration);
        }
        $('#info_boxbody').append(main);

        $(cls).hide().fadeIn(500);
        $(cls).find('input[type=text],textarea,select').filter(':visible:first').focus();
    } catch (err) {
        ErrorAlert(err, 2000);
    }

}


function appendInto(appendTo, overlayOn, html, title, modal, id) {
    try {
        var overlay;
        modal = Boolean(modal) || false;
        
        id = id || "";    

        if (id == "") {
            id = 'customDialog00' + $('.customDialog').length + 1;
        }

        var cls = '.customDialog#' + id;

        var main = document.createElement("div");
        main.setAttribute("class", "customDialog");
        main.setAttribute("id", id);
        main.setAttribute("overlay", overlayOn);
        main.setAttribute("style","position:relative;")

        

//        var overlayId = overlayOn.replace("#", "");
//        var nodes = document.getElementById(overlayId).getElementsByTagName('*');
//        for (var i = 0; i < nodes.length; i++) {
//            nodes[i].disabled = true;
//        }
        

        if (modal == true) {
            var overlay = document.createElement("div");
            overlay.setAttribute("id", "dialogoverlay");
            overlay.setAttribute("class", "overlay");
            overlay.setAttribute("style", "botton:auto;left:0px;top:0px; right:0px; background-color: #808080; z-index:1");
            overlay.setAttribute("parent", overlayOn);
            
        }

        var dlg = document.createElement("div");
        dlg.setAttribute("id", "dialog_Modal");
        dlg.setAttribute("class", "dialog_main");
//        dlg.setAttribute("tag", tag);
     
        
        dlg.setAttribute("style", "position:relative");

        dlg.innerHTML = dlg.innerHTML + "<span style='font-weight:bold; padding-bottom:15px;' >" + title + "<" + "/span>";
        dlg.innerHTML = dlg.innerHTML + "<span class='closeBox' id = 'removeAppended' style='font-weight:bold; float:right;'>&#x0058;<" + "/span>";
//        dlg.innerHTML = dlg.innerHTML + "<span class='restoreBox' id='restoreInner' style='font-weight:bold; float:right;'>&#9633;<" + "/span>";
        dlg.innerHTML = dlg.innerHTML + "<hrLineCalendar>";
        dlg.innerHTML = dlg.innerHTML + "<div class='dialog_container' id='divModal' style='bottom:auto; position:relative; margin: 0px 2px 2px 0px;'>" + html + "<" + "/div>"

        if (modal == true) {
            $(overlay).prependTo(overlayOn);
            $(overlay).css("height", $(overlayOn).css("height"));
            $(overlay).css("top", $(overlayOn).css("top"));
        }

        main.appendChild(dlg);

        $(appendTo).html("");
        $(appendTo).append(main);

        $(cls).hide().fadeIn(500);       

//        $(appendTo).find(".dialog_main").css("min-height", $(appendTo).find(".dialog_container").height() + 26); 


        var position = $(overlayOn).position();
       
        var top = position.top + $(appendTo).height()+ 500; 
        var scrolled = position.top;
        scrolled = scrolled + $(appendTo).height()+ 300;
        $(appendTo).closest(".dialog_container").animate({
            scrollTop: scrolled
        });

        $(appendTo).find('input[type=text],textarea,select').filter(':visible:first').focus();


    } catch (err) {
        ErrorAlert(err, 5000);
    }
}

$(window).resize(function () {
    $("#dialogoverlay").each(function () {
        var parent = $(this).attr("parent");
        var height = $(parent).height();
        $(this).css("height", height);
    });
});

function appendNormal(appendTo, html, title, position, duration) {
    try {
//        alert("A");
//        position = position || "61px,0px,0px,205px";
//        duration = duration || 0;

//        var pos = position.split(",");
//        var top = pos[0];
//        var right = pos[1];
//        var bottom = pos[2];
//        var left = pos[3];

        //        var tag = '0,' + position;

        position = position || "A,61px,0px,0px,205px";
        //        position = position || "R,20%,90px,50%,405px";
        id = id || "";
        duration = duration || 0;

        var VType = "A";
        var top = "";
        var right = "";
        var bottom = "";
        var left = "";
        var width = "";
        var maxHeight = "";

        var pos = position.split(",");

        if (pos.length == 4) {
            VType = "A";
            top = pos[0];
            right = pos[1];
            bottom = pos[2];
            left = pos[3];
        }
        else if (pos.length == 5) {
            VType = pos[0];
            if (VType == "A") {
                top = pos[1];
                right = pos[2];
                bottom = pos[3];
                left = pos[4];
            }
            else {
                left = pos[1];
                top = pos[2];
                width = pos[3];
                maxHeight = pos[4];
            }
        }


        var tag;
        if (pos.length == 5) {
            tag = '0,' + position;
        }
        else {
            tag = '0,A,' + position;
        }

        var id = 'dialog_Normal00' + $('.dialog_main').length + 1;

        var cls = '.dialog_main#' + id;


        //        var main = document.createElement("div");
        //        main.setAttribute("class", "customDialog");
        //        main.setAttribute("id", id);


        var dlg = document.createElement("div");
        dlg.setAttribute("class", "dialog_main");
        dlg.setAttribute("id", id);

        dlg.setAttribute("tag", tag);

        //        dlg.setAttribute("style", "left:" + left + ";right:" + right + ";top:" + top + ";bottom:" + bottom + ";");

        if (VType == "A") {
            dlg.setAttribute("style", "left:" + left + ";right:" + right + ";top:" + top + ";bottom:" + bottom + ";");
        }
        else {
            dlg.setAttribute("style", "position:relative; padding:1px; left:" + left + ";width:" + width + ";top:" + top + ";");
        }


        dlg.innerHTML = dlg.innerHTML + "<span style='font-weight:bold; padding-bottom:15px; margin-left:8px;' >" + title + "<" + "/span>";

        dlg.innerHTML = dlg.innerHTML + "<span class='closeBox' id = 'closeNormal' style='font-weight:bold; float:right;'>&#x0058;<" + "/span>";
        dlg.innerHTML = dlg.innerHTML + "<span class='restoreBox' id='restoreAppendNormal' style='font-weight:bold; float:right;'>&#9633;<" + "/span>";

        dlg.innerHTML = dlg.innerHTML + "<hrLineCalendar>";
        //        dlg.innerHTML = dlg.innerHTML + "<div class='dialog_container' id='divNormal'  style='overflow:auto; bottom:0px; z-index=1;'>" + html + "<" + "/div>"
        if (VType == "A") {
            dlg.innerHTML = dlg.innerHTML + "<div class='dialog_container' id='divModal'  style='overflow:auto; bottom:0px; z-index=1;' >" + html + "<" + "/div>"
        }
        else {
            dlg.innerHTML = dlg.innerHTML + "<div class='dialog_container' id='divModal' style='bottom:auto; position:relative; margin:0px 2px 2px 0px; max-height:" + maxHeight + ";' >" + html + "<" + "/div>"
        }

        if (duration != 0) {
            setTimeout(function () {
                dlg.parentNode.removeChild(dlg);
            }, duration);
        }
        $(appendTo).html("");
        $(appendTo).append(dlg);

        $(cls).hide().fadeIn(500);

        $(cls).find('input[type=text],textarea,select').filter(':visible:first').focus();

        //            var duration = 2000;
        //            if (duration != 0) {
        //                setTimeout(function () {
        //                    dlg.parentNode.removeChild(dlg);
        //                }, duration);
        //            }


        //        $(cls).find(".dialog_container#divNormal").mCustomScrollbar({
        //            scrollButtons: {
        //                enable: false
        //            },
        //            advanced: {
        //                updateOnContentResize: true
        //            }
        //        });
    } catch (err) {
        ErrorAlert(err, 2000);
    }
}




$(".closeBox").on("click", function (e) {
    if ($(this).attr("id") == "closeAlert") {
        $(this).parents("#dialog_alert").remove();
    }
    else if ($(this).attr("id") == "closeCalander") {
        CloseCalanderDialog();
    }
    else if ($(this).attr("id") == "removeAppended") {
        var overlayOn = $(this).closest(".customDialog").attr("overlay");
        var overlayId = overlayOn.replace("#", "");
        //var nodes = document.getElementById(overlayId).getElementsByTagName('*');
        //        for (var i = 0; i < nodes.length; i++) {
        //            nodes[i].disabled = false;
        //        }  
        var myId = $(this).closest(".customDialog").attr("id");
        $(overlayOn).find(".overlay").remove();
        $(this).closest(".customDialog").remove();

        try {
            appendIntoClosed(myId);
        } catch (err) { };

    }
    else if ($(this).attr("id") == "closeNormal") {
        $(this).parents(".dialog_main").remove();

        //$(this).parents(".customDialog").remove();
    }
    else if ($(this).attr("id") == "closeCustomSearch") {
        CloseCustomSearchDialog();
    }
    else if ($(this).attr("id") == "closeDialog" || $(this).attr("id") == "removeDialog") {
        $(this).closest(".customDialog").remove();
    }
    else if ($(this).attr("id") == "closeCustomerDialog") {
        CloseCustomerSearchDialog();
    }
    else if ($(this).attr("id") == "closeMessage") {
        $(this).parents(".customMsg").remove();
    }
    else {
        $(this).parents(".customDialog#customDialog*").remove();
    }
});


$(".restoreBox").on("click", function (e) {

    var objSearch;
    if ($(this).attr("id") == "restoreCustomSearch") {
        objSearch = $(this).parents(".dialog_search");
    }
    else {
        objSearch = $(this).parents(".dialog_main");
    }

    var oldPos = objSearch.attr("tag").split(",");
   
    var maxmize = parseInt(oldPos[0]);

    var VType = oldPos[1];
    var AtopRLeft = oldPos[2];
    var ArightRTop = oldPos[3];
    var AbottomRWidth = oldPos[4];
    var AleftRMaxWidth = oldPos[5];

    var newPos = "";

    if (maxmize == 0) {
        newPos = '1,' + VType + ',' + AtopRLeft.toString() + ',' + ArightRTop.toString() + ',' + AbottomRWidth.toString() + ',' + AleftRMaxWidth.toString();
    }
    else {
        newPos = '0,' + VType + ',' + AtopRLeft.toString() + ',' + ArightRTop.toString() + ',' + AbottomRWidth.toString() + ',' + AleftRMaxWidth.toString();
    }

    objSearch.attr("tag", newPos);



    if (maxmize == 0) {
        objSearch.css("top", "0px");
        objSearch.css("left", "0px");
        objSearch.css("right", "0px");
        objSearch.css("bottom", "0px");
        objSearch.css("width", "");
        if (VType != "A") {
            objSearch.css("position", "fixed");
            var dlgCont = objSearch.find(".dialog_container");
            $(dlgCont).css("position", "absolute");
            $(dlgCont).css("margin", "20px 0px 0px 0px");
            $(dlgCont).css("height", "");
            $(dlgCont).css("max-height", "");
            $(dlgCont).css("bottom", "0px");
        }

    }
    else {
        if (VType == "A") {
            objSearch.css("top", AtopRLeft);
            objSearch.css("left", AleftRMaxWidth);
            objSearch.css("right", ArightRTop);
            objSearch.css("bottom", AbottomRWidth);
        }
        else {
            objSearch.css("position", "relative");
            objSearch.css("width", AbottomRWidth);
            objSearch.css("left", AtopRLeft);
            objSearch.css("top", ArightRTop);
            objSearch.css("right", "");

            var dlgCont = objSearch.find(".dialog_container");
            $(dlgCont).css("position", "relative");
            $(dlgCont).css("margin", "0px 2px 2px 0px");
            $(dlgCont).css("max-height", AleftRMaxWidth);
            $(dlgCont).css("bottom", "auto");
        }
    }
});