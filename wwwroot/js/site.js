<<<<<<< Updated upstream
﻿var radio = document.querySelector('.manual-btn')
=======
﻿
var radio = document.querySelector('.manual-btn')
>>>>>>> Stashed changes
var cont = 1

document.getElementById('radio1').checked = true

setInterval(() => {
    proximaImg()
}, 5000)

<<<<<<< Updated upstream
function proximaImg() {
    cont++
    if (cont > 3) {
        cont = 1
    }
    document.getElementById('radio' + cont).checked = true
=======
function proximaImg(){
    cont++
    if(cont > 3){
        cont = 1
    }
    document.getElementById('radio'+cont).checked = true
>>>>>>> Stashed changes
}