var selectedObject = {
    "precision": {
        "2022": ["2022"],
        "2021": ["2021"],
        "2020": ["2020"]
    },
    "passes": {
        "2022": ["2022"],
        "2021": ["2021"],
        "2020": ["2020"]
    }
}

//Function for 2 depending dropdowns for statistic and year for chart
window.onload = function () {
    var statisticSel = document.getElementById("statistics");
    var yearsSel = document.getElementById("years");
    for (var x in selectedObject) {
        statisticSel.options[statisticSel.options.length] = new Option(x);
    }
    statisticSel.onchange = function () {
        //empty statistics- and years- dropdowns
        yearsSel.length = 1;
        //display correct values
        for (var y in selectedObject[this.value]) {
            yearsSel.options[yearsSel.options.length] = new Option(y, y);
        }
    }
}

function dropdown(value, year) {
    let xhr = new XMLHttpRequest();
    let link = "/Home/Chart?value=" + value + "&datevalue=" + year
    xhr.open("get", link, true);
    xhr.send(null);
    xhr.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let response = JSON.parse(this.response);
                data.datasets[0].data = response
                sessionChart.update();
                
            }
        }
    }
}

//Gets options from dropdown
function getOptions() {
    let statisticOpt = document.getElementById("statistics").value;
    let yearOpt = document.getElementById("years").value;
    dropdown(statisticOpt, yearOpt);
}