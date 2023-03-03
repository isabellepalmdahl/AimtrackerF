
let heatMapColor = 240;
function createCanvas(size) {
    let canvas = document.createElement("CANVAS");
    let obj = document.getElementById('heatmapContainer');
    let obj2 = document.getElementById('heatmapColorExplain');
    let canvasColor = document.createElement("CANVAS");
    canvasColor.width = 50;
    canvasColor.height = 200;
    canvas.width = size;
    canvas.height = size;
    canvas.style.border = "2px solid black";
    canvas.style.borderRadius = "1rem";
    canvasColor.style.border = "2px solid black";
    canvas.style.width = "400px";
    canvas.style.height = "400px";
    canvas.id = "heatMap";
    canvasColor.id = "heatMapColor";
  
    obj.appendChild(canvas); 
    obj2.appendChild(canvasColor);
}
function generateHeatMap(precision, list,type) {
    let canvas = document.getElementById('heatMap')
    let canvasColor = document.getElementById('heatMapColor')
    if (canvas != undefined && canvasColor != undefined) {
        canvas.getContext("2d").clearRect(0, 0, canvas.width, canvas.height);
        canvasColor.getContext("2d").clearRect(0, 0, canvas.width, canvas.height);
        let shotsList = getShots(list, 400,type); 
        let matrix = heatMap(shotsList, 400, precision);
        applyHeatMapColorExplain(canvasColor);
        canvasColor.style.transform = "rotate(180deg)";
        applyHeatMap(canvas, matrix);
        createTarget(canvas);
        
    }
   
}
function applyHeatMapColorExplain(canvas) {
    for (let i = 0; i < 99; i++) {
        createPolygon(canvas, 0, (canvas.height / 100) * i, canvas.width, (canvas.height / 100) * i + 1, getHeatMapColor(i/99));
    }
}
function updateHeatmap() {
    let precision = document.getElementById('heatmapPrecision').value;
    if (newSessionData == undefined)
        generateHeatMap(precision, sessionData,"Sessions");
    else
        generateHeatMap(precision, newSessionData,"Shots");
}
//HeatMap Data
function heatMap(shotsList, size, precision) {
    let matrix = generateMatrix(size);
    matrix = fillMatrixWithCordData(matrix, shotsList, precision);
    return matrix;
}
function generateMatrix(precision) {
    //Generates a grid/matrix like:
    // precision = 4
    // [[0, 0, 0, 0],
    //  [0, 0, 0, 0],
    //  [0, 0, 0, 0]]
    //Where first array is rows/y and array in array is column/x to map cords to
    //precision = size 
    let matrix = [];
    for (let i = 0; i < precision; i++) {
        let array = [];
        matrix.push(array);
        for (let n = 0; n < precision; n++) {
            matrix[i].push(0);
        }
    }
    return matrix;
}
function fillMatrixWithCordData(matrix, list, precision) {
    //0-400 cord value
    list.forEach(shot => {
        let x = shot.x;
        let y = shot.y;
        if (matrix[y] == undefined)
            console.log("Undefined Y",y)
        else if (matrix[y][x] == undefined)
            console.log("Undefined X",x)
        matrix[y][x]++;

        
    });
    matrix = scaleDownMatrix(matrix, precision);
    let maxValue = 0;
    let minValue = 1000000000;
    matrix.forEach(row => {
        row.forEach(cell => {
            if (cell > maxValue) {
                maxValue = cell;
            }
            if (cell < minValue) {
                minValue = cell;
            }
        })
    });
    //Normalize values to span 0-10;
    for (let i = 0; i < matrix.length-1; i++) {
        for (let n = 0; n < matrix.length; n++) {
            matrix[i][n] = normalizeValue(matrix[i][n], minValue, maxValue);
        }
    }
    return matrix;
}
function updateHeatmapColor(obj){
    heatMapColor = obj.value;
    updateHeatmap();
}
function getHeatMapColor(value) {
    let hue = Math.round(((1 - value) * heatMapColor));
   2
        return ["hsl(", hue, ",100%,50%)"].join("");
}
function getShots(list, precision, type) {
    let shotList = [];
    switch (type) {
        case "Sessions":
            list.forEach(session => {
                session.results.forEach(series => {
                    series.shots.forEach(shot => {
                        let item = { x: Math.round(((shot.shotXCord + 200))), y: Math.round(((shot.shotYCord + 200))) };
                        if (item.x >= precision)
                            item.x = precision - 1;
                        if (item.y >= precision)
                            item.y = precision - 1;
                        if (item.x > precision || item.y > precision)
                            console.log("TOO HIGH")
                        else if (item.x < 0 || item.y < 0)
                            console.log("TOO LOW")
                        shotList.push(item);
                    });
                });
            });
            break;
        case "Series":
            break;
        case "Shots":
            list.forEach(shot => {
                let item = { x: Math.round(((shot.shotXCord + 200))), y: Math.round(((shot.shotYCord + 200))) };
                if (item.x >= precision)
                    item.x = precision - 1;
                if (item.y >= precision)
                    item.y = precision - 1;
                if (item.x > precision || item.y > precision)
                    console.log("TOO HIGH")
                else if (item.x < 0 || item.y < 0)
                    console.log("TOO LOW")
                shotList.push(item);
            });
            break;
}
    return shotList;
}
function scaleDownMatrix(matrix, factor) {
    let newMatrix = matrix;
    for (let i = 0; i < factor; i++) {
        newMatrix = matrixShrink(newMatrix);
    }
    return newMatrix;
}
function matrixShrink(matrix) {
  
    let newMatrix = generateMatrix(matrix.length / 2);
    for (let i = 0; i < newMatrix.length-1; i++) {
        for (let n = 0; n < newMatrix.length; n++) {
            //Combines values in grid like 
            //[1, 1]
            //[1, 1]
            //= [4]

             newMatrix[i][n] = matrix[i*2][n * 2] + matrix[i*2][n * 2 + 1] + matrix[i * 2+1][n * 2] + matrix[i * 2 + 1][n * 2 + 1];
        }
    }
    return newMatrix;
}
//Data
function normalizeValue(value, min, max) {
    
    return ((value - min) / (max - min));
}
function generateRandomNumber(min, max) {
    return Math.floor(Math.random() * (max - min + 1) + min);
}

//Canvas Drawing
function applyHeatMap(canvas, matrix) {
    
    let cellSize = (canvas.width / matrix.length);
    for (let i = 0; i < matrix.length; i++) {
        for (let n = 0; n < matrix.length; n++) {

            let startY = i * cellSize;
            let startX = n * cellSize;
            let targetY = (i * cellSize) + cellSize;
            let targetX = (n * cellSize) + cellSize;
             
            let color = getHeatMapColor(matrix[i][n]);
            if (matrix[i][n] >0.0075) {
                createPolygon(canvas, startX, startY, targetX, targetY, color,"Fill");
            } else {
                createPolygon(canvas, startX, startY, targetX, targetY, "white","Fill");
            }
            
        }
    }
          
}
function createTarget(canvas) {
    createCircle(canvas, canvas.width / 2, canvas.height / 2, canvas.width / 2, "Black", "Stroke");
    createCircle(canvas, canvas.width / 2, canvas.height / 2, canvas.width / 2 + 1, "Black", "Stroke");
    createCircle(canvas, canvas.width / 2, canvas.height / 2, canvas.width / 2 + 2, "Black", "Stroke");
    createCircle(canvas, canvas.width / 2, canvas.height / 2, (canvas.width / 2) * 0.575, "Black", "Stroke");
    createCircle(canvas, canvas.width / 2, canvas.height / 2, (canvas.width / 2) * 0.575 - 1, "Black", "Stroke");
    createCircle(canvas, canvas.width / 2, canvas.height / 2, (canvas.width / 2) * 0.575 - 2, "Black", "Stroke");
    createCircle(canvas, canvas.width / 2, canvas.height / 2, (canvas.width / 2) * 0.075 - 1, "Black", "Stroke");
    createCircle(canvas, canvas.width / 2, canvas.height / 2, (canvas.width / 2) * 0.075 - 2, "Black", "Stroke");
    createCircle(canvas, canvas.width / 2, canvas.height / 2, (canvas.width / 2) * 0.075 - 3, "Black", "Stroke");
    createLine(canvas, canvas.width / 2, 0, canvas.width / 2, canvas.height, "Black");
    createLine(canvas, 0, canvas.width / 2, canvas.height, canvas.width / 2, "Black");
}
function createLine(canvas,startX,startY,targetX,targetY,color) {
    let ctx = canvas.getContext("2d");
    ctx.strokeStyle = color;
    ctx.moveTo(startX, startY);
    ctx.lineTo(targetX, targetY);
    ctx.stroke();
}
function createCircle(canvas, x, y, size, color, type) {
    let ctx = canvas.getContext("2d");
    ctx.beginPath();
    ctx.arc(x, y, size, 0, 2 * Math.PI);
    ctx.closePath();
    switch (type) {
        case "Stroke":
            ctx.strokeStyle = color
            ctx.stroke();
            break;
        case "Fill":
            ctx.fillStyle = color
            ctx.fill();
            break;
    }
}
function createPolygon(canvas, startX, startY, targetX, targetY, color) {
    let ctx = canvas.getContext("2d");
    ctx.fillStyle = color;
    ctx.fillRect(startX, startY, targetX, targetY);
}
