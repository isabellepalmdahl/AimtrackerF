
data.datasets[0].data = monthWithShootings;

const filterChart = new Chart(
    document.getElementById('filterchart'),
    config,

);
let sizePulse = 85;
let pulseMax = 220; 

function FindingAllShots(list) {
    let array = [];
    list.forEach(session => { session.results.forEach(series => { series.shots.forEach(shot => { array.push(shot) }) }) })

    return array;
}
function FindingAllPulse(list) {
    let array = [[], []];
    for (let i = 0; i < pulseMax; i++) {
        array[0].push(0);
        array[1].push(0);
    }
    for (let j = 0; j < list.length; j++) {
        if (list[j].result === true)
            array[0][list[j].heartRate]++
        else
            array[1][list[j].heartRate]++
    }
    return array;
}
let pulsedata = FindingAllPulse(FindingAllShots(sessionData));
let pulseXaxel = []
let pulseaxel = [[], []]
for (let n = sizePulse; n < pulseMax; n++) {
    pulseXaxel.push(n);
    pulseaxel[0].push(pulsedata[0][n]);
    pulseaxel[1].push(pulsedata[1][n]);
}
filterChart.data.labels = pulseXaxel

data.datasets[0].data = pulseaxel[0];
data.datasets[0].label = 'Hits'
data.datasets.push({
    label: 'Miss',
    backgroundColor: 'blue',
    borderColor: 'blue',
    data: pulseaxel[1]
})
filterChart.update();


function updateFilterChart() {
    sizePulse = document.getElementById('heartrateinputfrom').value;
    pulseMax = document.getElementById('heartrateinputto').value;
    let start = Number.parseInt(sizePulse);
    let max = Number.parseInt(pulseMax);

    let Xaxel = [];
    for (let m = start; m < max+1; m++) {
        Xaxel.push(m);
    }
    let pulsedata = [[], []];
    for (let b = 0; b < max-start+1; b++) {
        pulsedata[0].push(0);
        pulsedata[1].push(0);
    }

    for (let a = 0; a < newSessionData.length; a++) {
        if (newSessionData[a].result === true) {
            pulsedata[0][newSessionData[a].heartRate-start]++;
        }
        else {
            pulsedata[1][newSessionData[a].heartRate - start]++;
        }
    }
    data.datasets[0].data = pulsedata[0];
    data.labels = Xaxel;
    data.datasets[1].data = pulsedata[1];
    filterChart.update();

}