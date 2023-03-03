const labels = [
    'January',
    'February',
    'March',
    'April',
    'May',
    'June',
    'July',
    'August',
    'September',
    'October',
    'November',
    'December'
    ];

let data = {
    labels: labels,
    datasets: [{
        label: 'My sessions',
        backgroundColor: 'rgb(255, 99, 132)',
        borderColor: 'rgb(255, 99, 132)',
    }]
};

const config = {
    type: 'line',
    data: data,
    options: {}
};
