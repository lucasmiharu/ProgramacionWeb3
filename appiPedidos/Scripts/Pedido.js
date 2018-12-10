$(document).on("ready", function () {


    $('#btnConfirmarGustos').on('click', function () {
        var pedido = new Object();

        alert("hola");

        pedido.IdUsuario = 1;
        ////pedido.Token = "767EE897-D6DC-47E3-ABB9-4E0309B54D89";
        pedido.IdPedido = 1;
        pedido.IdGustoEmpanada = 1;
        pedido.Cantidad = 3;

        CrearPedido(JSON.stringify(pedido));
    })
   
})



////Create a new person
function CrearPedido(pedido) {

    alert(pedido);

    var url = '/api/pedido/';
    $.ajax({
        url: url,
        type: 'POST',
        data: pedido,
        contentType: "application/json;chartset=utf-8",
        statusCode: {
            201: function () {
                // GetAll();
                //  ClearForm();
                alert('Person with id: was updated');
            },
            400: function () {
                //  ClearForm();
                alert('Error');
            }
        }
    });
};

