var x, i, j, l, ll, selElmnt, a, b, c;
/* Look for any elements with the class "dropdown": */
x = document.getElementsByClassName("dropdown");
l = x.length;
let values = ["week", "month", "year"];
for (i = 0; i < l; i++) {
    selElmnt = x[i].getElementsByTagName("select")[0];
    ll = selElmnt.length;
    /* For each element, create a new DIV that will act as the selected item: */
    a = document.createElement("DIV");
    a.setAttribute("class", "select-selected");
    a.innerHTML = selElmnt.options[selElmnt.selectedIndex].innerHTML;
    
    x[i].appendChild(a);
    /* For each element, create a new DIV that will contain the option list: */
    b = document.createElement("DIV");
    b.setAttribute("class", "select-items select-hide");
    b.setAttribute("id", "dropdownValues");
    for (j = 1; j < ll; j++) {
        /* For each option in the original select element,
        create a new DIV that will act as an option item: */
        c = document.createElement("DIV");
        c.setAttribute("id", "dropdownOption" + j);
        c.setAttribute("value",values[j-1])
        c.innerHTML = selElmnt.options[j].innerHTML;
        c.addEventListener("click", function (e) {
            /* When an item is clicked, update the original select box,
            and the selected item: */
            var y, i, k, s, h, sl, yl;
            //HttpGet for sessions
            changeSessionTimeSpan(this.getAttribute('value'));
            let vsDoc = document.getElementsByClassName('vsstatistics');
            //Replaces last week etc
            for (let element of vsDoc) {
                let original = element.innerText;

                let target = this.innerText.substring(2);
                let extra = "";
                if (target.includes("vecka")) {
                    extra = "n";
                } else if (target.includes("månad")) {
                    extra = "en";
                } else if (target.includes("år")) {
                    extra = "et";
                }
                let strStart = "";
                for (let char of original) {
                    if (char != " ")
                        strStart = strStart + char;
                    else
                        break;
                }
                strStart = strStart +" " + target + extra;
                element.innerText = strStart;
            };
            s = this.parentNode.parentNode.getElementsByTagName("select")[0];
            sl = s.length;
            h = this.parentNode.previousSibling;
            for (i = 0; i < sl; i++) {
                if (s.options[i].innerHTML == this.innerHTML) {
                    s.selectedIndex = i;
                    h.innerHTML = this.innerHTML;
                    y = this.parentNode.getElementsByClassName("same-as-selected");
                    yl = y.length;
                    for (k = 0; k < yl; k++) {
                        y[k].removeAttribute("class");
                    }
                    this.setAttribute("class", "same-as-selected");
                    break;
                }
            }
            h.click();
        });
        b.appendChild(c);
    }
    x[i].appendChild(b);
    a.addEventListener("click", function (e) {
        /* When the select box is clicked, close any other select boxes,
        and open/close the current select box: */
        e.stopPropagation();
        closeAllSelect(this);
        
        this.nextSibling.classList.toggle("select-hide");
        this.classList.toggle("select-arrow-active");
    });
}
function closeAllSelect(elmnt) {
    /* A function that will close all select boxes in the document,
    except the current select box: */
    /* changes dropdown border-radius*/
    if (elmnt.tagName == "DIV") {
        
        if (elmnt.style.borderRadius == "15px" || elmnt.style.borderRadius == "")
            elmnt.style.borderRadius = "15px 15px 0px 0px";
        else
            elmnt.style.borderRadius = "15px";
        
    }

    var x, y, i, xl, yl, arrNo = [];
    x = document.getElementsByClassName("select-items");
    y = document.getElementsByClassName("select-selected");
    xl = x.length;
    yl = y.length;
    for (i = 0; i < yl; i++) {
        if (elmnt == y[i]) {
            arrNo.push(i)
        } else {
            y[i].classList.remove("select-arrow-active");
            y[i].style.borderRadius = "15px";
            
        }
    }
    for (i = 0; i < xl; i++) {
        if (arrNo.indexOf(i)) {
            x[i].classList.add("select-hide");
        }
    }
    
}

/* If the user clicks anywhere outside the select box,
then close all select boxes: */
document.addEventListener("click", closeAllSelect);

function changeSessionTimeSpan(obj) {
    inputComment = document.getElementById('comment');
    inputShootingId = document.getElementById('shootingId');

    let value = obj;
    let xhr = new XMLHttpRequest();
    let link = "/Home/Time?value=" + value
    xhr.open('Get', link, true);
    xhr.send(null);
    xhr.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {

                let doc = document.getElementById('sessions');

                let elementsToRemove = [];
                doc.childNodes[1].childNodes.forEach(element => {
                    if (element.id != "sessions-th") {
                        elementsToRemove.push(element);
                    }
                });
                elementsToRemove.forEach(element => {
                    doc.childNodes[1].removeChild(element);
                })
                let model = JSON.parse(this.response);
                let totalHitStatistic = 0;
                let totalHitStatisticVS = 0;
                model.sessions = sortSessionsByDate(model.sessions);
                model.sessions.forEach(element => {
                    let date = new Date(element.date).toLocaleDateString();
                    let time = new Date(element.date).toLocaleTimeString();
                    let hitStatistic = element.hitStatistic
                    totalHitStatistic += hitStatistic;
                    let htmlCode = "";
                    if (element.comments === null)
                        htmlCode = `<tr class='clickable-tr trhoverable' onclick="window.location.href='/Session/Session/${element.shootingId}';" ><td>${date} ${time}</td ><td>${hitStatistic}%</td><td></td></ tr>`
                    else
                        htmlCode = `<tr class='clickable-tr trhoverable' onclick="window.location.href='/Session/Session/${element.shootingId}';" ><td>${date} ${time}</td ><td>${hitStatistic}%</td><td class="commenttd" data-hover="${element.comments}"><i class ="fa-solid fa-comment"></i></td></ tr>`

                    doc.childNodes[1].insertAdjacentHTML("beforeend", htmlCode);
                })
                model.sessionsVS.forEach(element => {
                    totalHitStatisticVS += element.hitStatistic;
                });
                totalHitStatistic = Math.round((totalHitStatistic / model.sessions.length) * 10) / 10;
                totalHitStatisticVS = Math.round((totalHitStatisticVS / model.sessionsVS.length) * 10) / 10;
                if (!Number.isNaN(Math.round((totalHitStatistic / model.sessions.length) * 10) / 10))
                    document.getElementById('hit-statistics').innerText = totalHitStatistic+ "%";
                else
                    document.getElementById('hit-statistics').innerText = "0%";
                if (!Number.isNaN(Math.round((totalHitStatisticVS / model.sessionsVS.length) * 10) / 10))
                    document.getElementById('hit-statisticsVS').innerText = "vs. " + totalHitStatisticVS + "%";
                else
                    document.getElementById('hit-statisticsVS').innerText ="vs. 0%";
                document.getElementById('numberOfSessions').innerText = model.sessions.length;
                document.getElementById('numberOfSessionsVS').innerText = "vs. " + model.sessionsVS.length;
               
                document.getElementById('hitStatPercent').innerText = model.hitStatPrChange+ "%";
                document.getElementById('sessionsPercent').innerText = model.sessionsPrChange+ "%";
            }
        }
    }
}
const sortByDate = arr => {
    const sorter = (a, b) => {
        return new Date(b.date).getTime() - new Date(a.date).getTime();
    }
    arr.sort(sorter);
};
function sortSessionsByDate(sessions) {
    sortByDate(sessions);
    return sessions;
}
