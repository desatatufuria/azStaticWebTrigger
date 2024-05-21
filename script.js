document.getElementById('moodForm').addEventListener('submit', function(event) {
    event.preventDefault();

    const name = document.getElementById('name').value;
    const mood = document.getElementById('mood').value;

    fetch('api/MoodHttpTrigger', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ name: name, mood: mood })
    })
    .then(response => response.text())
    .then(data => {
        document.getElementById('responseMessage').innerText = data;
    })
    .catch(error => {
        console.error('Error:', error);
    });
});
