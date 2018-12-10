function validacionCheck(Listachecks, obj) {
    limite = 1; //limite de checks a seleccionar
    num = 0;

    for (i = 0; ele = document.getElementById(Listachecks).children[i]; i++) {
        if (ele.checked)
            num++;
    }
    if (num < limite) {
        alert('Debe seleccionar los invitados de la lista');
        return false;
    }
    else {
        return true;
    }

}

$(document).ready(function () {

    

    var navListItems = $('div.setup-panel div a'),
        allWells = $('.setup-content'),
        allNextBtn = $('.nextBtn');

    allWells.hide();

    navListItems.click(function (e) {
        e.preventDefault();
        var $target = $($(this).attr('href')),
            $item = $(this);

        if (!$item.hasClass('disabled')) {
            navListItems.removeClass('btn-primary').addClass('btn-default');
            $item.addClass('btn-primary');
            allWells.hide();
            $target.show();
            $target.find('input:eq(0)').focus();
        }
    });

    allNextBtn.click(function () {
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            curInputs = curStep.find("input[type='text'],input[type='url']"),
            isValid = true;

        $(".form-group").removeClass("has-error");
        for (var i = 0; i < curInputs.length; i++) {
            if (!curInputs[i].validity.valid) {
                isValid = false;
                $(curInputs[i]).closest(".form-group").addClass("has-error");
            }
        }

        if (isValid)
            nextStepWizard.removeAttr('disabled').trigger('click');
    });

    $('div.setup-panel div a.btn-primary').trigger('click');



    $('#btnConfirmarGustos').on('click', function () {
        var pedido = new Object();
          
        pedido.IdPedido = $('#idPedido').val();
        pedido.IdUsuario = $('#IdUsuario').val();
     //   pedido.Token = $('#Token').val();       
       var GustosEmpanadasCantidad = [];
       

        $("input[name=Cantidad]").each(function () {      
            GustosEmpanadasCantidad.push({ "IdGustoEmpanada": this.id, "Cantidad": $('#' + this.id).val() });    
        })
        
        pedido.GustosEmpanadasCantidad = GustosEmpanadasCantidad;    

              
        ElegirGustos(JSON.stringify(pedido));
    })

});


//seleccionar gustos
function ElegirGustos(pedido) {
    alert(pedido);
    var url = 'http://localhost:59117/api/pedido/';
    $.ajax({
        url: url,
       // dataType: "jsonp",
        type: 'POST',
       data: pedido,
        contentType: "application/json;chartset=utf-8",
        success: function (result) {
            var mensajeResult = JSON.parse(result);
            $('#headerResultado').html(mensajeResult.Resultado);
            $('#mensajeGustos').html(mensajeResult.Mensaje);
            $("#confirmacionGustosModal").modal();
        }        
    });
}




$(document).ready(function () {
    $('#datatable').dataTable();

    $("[data-toggle=tooltip]").tooltip();
});

