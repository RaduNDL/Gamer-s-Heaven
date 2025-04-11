document.addEventListener("DOMContentLoaded", function () {
    const gameId = document.querySelector(".favorite-star").getAttribute("data-game");

    fetch(`/reviews/${gameId}`)
        .then(res => res.json())
        .then(reviews => {
            const reviewList = document.getElementById("review-list");
            reviews.forEach(r => {
                const div = document.createElement("div");
                div.classList.add("review-item");
                div.innerHTML = `<strong>${r.rating}★</strong> - ${r.comment}<br/><em>${new Date(r.date).toLocaleString()}</em>`;
                reviewList.appendChild(div);
            });
        });

    document.getElementById("submit-review").addEventListener("click", async () => {
        const comment = document.getElementById("review-text").value.trim();
        if (!comment) return;

        const response = await fetch('/reviews/add', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ gameID: gameId, comment: comment, rating: 5 })
        });

        if (response.ok) {
            location.reload();
        }
    });

    const star = document.querySelector(".favorite-star");
    const favorites = JSON.parse(localStorage.getItem("favorites") || "[]");

    if (favorites.includes(gameId)) {
        star.classList.add("favorited");
        star.innerHTML = "★";
    }

    star.addEventListener("click", () => {
        let favorites = JSON.parse(localStorage.getItem("favorites") || "[]");
        if (favorites.includes(gameId)) {
            favorites = favorites.filter(id => id !== gameId);
            star.innerHTML = "☆";
            star.classList.remove("favorited");
        } else {
            favorites.push(gameId);
            star.innerHTML = "★";
            star.classList.add("favorited");
        }
        localStorage.setItem("favorites", JSON.stringify(favorites));
    });
});
