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