document.addEventListener("DOMContentLoaded", async () => {
    const badge = document.getElementById("cart-count");

    if (!badge) return;

    try {
        const res = await fetch('/Cart/GetItemCount');
        const { count } = await res.json();
        badge.textContent = count;
    } catch {
        badge.textContent = "?";
    }
});
