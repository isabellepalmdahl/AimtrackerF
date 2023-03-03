function ChangeButton() {
    btn = document.getElementById("commentbtn");

    if (document.getElementById("comment").value == 0) {
        btn.innerHTML = 'Add';
    }
    else {
        btn.innerHTML = 'Modify';
    }
}

function addComment() {
    inputComment = document.getElementById("comment");
    inputShootingId = document.getElementById("shootid");
    let xhr = new XMLHttpRequest();
    let link = "/Session/AddCommentToShooting?ShootingId=" + inputShootingId.value + "&comment=" + inputComment.value;
    xhr.open('POST', link, true);
    xhr.send(null);
    xhr.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                console.log(JSON.parse(this.response));
            }
        }
    }
}

var rows = document.getElementById('series-table').getElementsByTagName('tr');
for (var i = 0; i < rows.length; i++) {
    var currentRow = rows[i];
    var createHandler = function (row) {
        return function () {
            var weatherId = row.querySelector('.weatherId').value;

            let xhr = new XMLHttpRequest();
            let link = "/Session/GetWeather?WeatherId=" + weatherId;
            xhr.open('GET', link, true);
            xhr.send(null);
            xhr.onreadystatechange = function () {
                if (this.readyState === 4) {
                    if (this.status === 200) {
                        var weather = JSON.parse(this.response);
                        document.getElementById('weatherDesc').innerText = weather.description;
                        document.getElementById('temp').innerText = `${weather.temp} C`;
                        document.getElementById('windspeed').innerText = weather.wind_speed;
                        document.getElementById('weatherIcon').src = `https://openweathermap.org/img/wn/${weather.icon}@2x.png`;
                        document.getElementById('arrow').style = `transform: rotate(${getWindDirection(weather.wind_deg)}deg)`;
                    }
                }
            }
        }
    }
    currentRow.onclick = createHandler(currentRow);
}

function getWindDirection(windDeg) {
    windDirection = windDeg - 323.8; /*this value will be added to db in future update*/
    if (windDirection < 0) {
        windDirection += 360;
    }
    return windDirection;
}

var shotCells = document.getElementById('series-table').getElementsByClassName('shotCell');
for (var i = 0; i < shotCells.length; i++) {
    var currentCell = shotCells[i];
    var createHandler = function (cell) {
        return function () {
            var heartRate = cell.querySelector('.shotHr').value;
            document.getElementById('pulseBpm').innerText = `${heartRate} BPM`;
            aimmovement.style.visibility = 'visible';
            timeinterval.style.visibility = 'visible';
            pulse.style.visibility = 'visible';
            weather.style.visibility = 'visible';
        }
    }
    currentCell.onclick = createHandler(currentCell);
}
function giveBorder(obj) {
    let elements = document.getElementsByClassName('shotBorder');
    if (elements.length > 0) {
        for (let i = 0; i < elements.length; i++) {
            elements[i].classList.remove("shotBorder")
        }
    }
    obj.parentElement.classList.add('shotBorder');
}