function ShowEditData() {
                var url = 'Home/Add';
    
                var rows = document.getElementById("mytable").rows;
                for (var i = 0, ceiling = rows.length; i < ceiling; i++) {
                    rows[i].onclick = function () {
                        debugger;
                        var CalorieItemId = this.cells[0].innerHTML;
                        var selectElement = document.getElementById("dccId");
                        var dccId=selectElement.value;
                        //var dccId = 1;

                        
                      //  var dccId = this.cells[6].childNodes[1].value;
                       // var dccId = this.cells[6].innerHTML.childNodes[1].value;
                        
                        window.location.href = url + '?cal_id=' + CalorieItemId + '&dcc_id=' + dccId;
                    }
                }

}

function confirmDelete(id, isDeleteChecked) {
    var deleteSpan = 'deleteSpan_' + id;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + id;

    if (isDeleteChecked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}



function alertMe() {
    alert("me");
}

