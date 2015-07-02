var checked = 0;


function checkAll() {
    var doms = document.getElementsByName("Id");
    if (checked == 0) {
        for (var i = 0; i < doms.length; i++) {
            doms[i].checked = "checked";
            checkthis(doms[i]);
        }
        checked = 1;
    } else {
        for (var j = 0; j < doms.length; j++) {
            doms[j].checked = "";
            checkthis(doms[j]);
        }
        checked = 0;
    }
}
function checkthis(dom) {

    if (dom.checked) {
        dom.parentElement.parentElement.style.backgroundColor = "#FFEE66";
    } else {
        dom.parentElement.parentElement.style.backgroundColor = "#FFFFFF";
    }
}

function getSelectedDoms(name) {
    var arr = [];
    if (name == null || name == "") name = "Id";
    var doms = document.getElementsByName(name);
    for (var i = 0; i < doms.length; i++) {
        if (doms[i].checked) {
            arr.push(doms[i]);
        }
    }
    return arr;
}

function getAllDataDoms(name) {
    var arr = [];
    if (name == null || name == "") name = "Id";
    var doms = document.getElementsByName(name);
    for (var i = 0; i < doms.length; i++) {
        arr.push(doms[i]);
    }
    return arr;
}


function getSelectedDatas(name) {
   
        var data = '{"datas":[';

        var doms = getSelectedDoms(name);
        for (var j = 0; j < doms.length; j++) {
            var inputs = doms[j].parentElement.parentElement.getElementsByTagName("input");

            if (inputs.length == 0) {
                return null;
            }
            data += '{';

            var dataLength = inputs.length;
            for (var i = 1; i <= dataLength; i++) {
                if (i == dataLength) {
                    data += '"' + inputs[i - 1].name + '":"' + inputs[i - 1].value + '"';
                } else {
                    data += '"' + inputs[i - 1].name + '":"' + inputs[i - 1].value + '",';
                }
            }
            data = data + "},";
        }
        data = data.substr(0, data.length - 1) + "]}";
    return data;
}
function getAllDatas() {

    var data = '{"datas":[';

    var doms = getAllDataDoms();
    for (var j = 0; j < doms.length; j++) {
        var inputs = doms[j].parentElement.parentElement.getElementsByTagName("input");

        if (inputs.length == 0) {
            return null;
        }
        data += '{';

        var dataLength = inputs.length;
        for (var i = 1; i <= dataLength; i++) {
            if (i == dataLength) {
                data += '"' + inputs[i - 1].name + '":"' + inputs[i - 1].value + '"';
            } else {
                data += '"' + inputs[i - 1].name + '":"' + inputs[i - 1].value + '",';
            }
        }
        data = data + "},";
    }
    data = data.substr(0, data.length - 1) + "]}";
    return data;
}


function getSelectedDatasNotIncludeHead() {
    var data = getSelectedDatas();
    data = data.substr(9, data.length - 10);
    return data;
}
function getAllDatasNotIncludeHead() {
    var data = getAllDatas();
    data = data.substr(9, data.length - 10);
    return data;
}

function CreateRow() {
    var htmlStr = $("#table tr").eq(1).html();
    $("#table tr:eq(0)").after("<tr>" + htmlStr + "</tr>");
    $("#table tr:eq(1)").find("input").val("");

}