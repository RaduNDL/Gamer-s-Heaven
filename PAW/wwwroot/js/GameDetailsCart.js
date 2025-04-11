document.addEventListener("DOMContentLoaded", () => {
    const button = document.getElementById("add-to-cart-direct");

    if (!button) return;

    button.addEventListener("click", async () => {
        const gameId = button.getAttribute("data-gameid");

        try {
            const res = await fetch('/Cart/AddToCart', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ gameId: parseInt(gameId) })
            });

            if (res.ok) {
                const result = await res.json();
                alert(result.message || "Added to cart!");
            } else {
                alert("Failed to add to cart.");
            }
        } catch (err) {
            console.error("Error adding to cart:", err);
            alert("Something went wrong.");
        }
    });
});
