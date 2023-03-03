let newSessionData;
function FilterStuff() {

    let windfrom = document.getElementById("windfrom").value;
    let windto = document.getElementById("windto").value;
    let tempfrom = document.getElementById("tempfrom").value;
    let tempto = document.getElementById("tempto").value;
    let heartrateinputfrom = document.getElementById("heartrateinputfrom").value;
    let heartrateinputto = document.getElementById("heartrateinputto").value;


    windfrom = (windfrom == "" ? "Notused" : windfrom)
    windto = (windto == "" ? "Notused" : windto)
    tempfrom = (tempfrom == "" ? "Notused" : tempfrom)
    tempto = (tempto == "" ? "Notused" : tempto)
    heartrateinputfrom = (heartrateinputfrom == "" ? "Notused" : heartrateinputfrom)
    heartrateinputto = (heartrateinputto == "" ? "Notused" : heartrateinputto)

    let xhr = new XMLHttpRequest();
    let link = `/Filter/GetSessionBasedOnHeartrate?heartrateFrom=${heartrateinputfrom}&heartrateTo=${heartrateinputto}&windfrom=${windfrom}&windto=${windto}&tempfrom=${tempfrom}&tempto=${tempto}`
   

    xhr.open('GET', link, true);
    xhr.send(null);
    xhr.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                newSessionData = JSON.parse(this.response);
                updateHeatmap();
                updateFilterChart();
            }
        }
    }
}

