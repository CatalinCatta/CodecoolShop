async function addToCart (id) {
    await fetch(`/Product/AddToCart/${id}`);
}

async function editNumber (id, number) {
    let response = await fetch(`/Product/AdjustCartItemNumber/${id}?number=${number}`);
    let result = await response.json();
    for(let i = 1; i < 50; i++){
        document.getElementById(`select-id-${id}-nr-${i}`).classList.remove("active");
    }
    let USDollar = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
    });
    let numberObject=document.getElementById(`select-id-${id}-nr-${number}`);
    let cartItems = document.getElementById("cart-items-number");
    
    numberObject.classList.add("active");
    numberObject.parentElement.parentElement.getElementsByClassName("btn")[0].innerHTML=number.toString();
    numberObject.parentElement.parentElement.querySelector("STRONG").innerHTML = "Price: " + USDollar.format(result.ProductPrice);

    cartItems.innerText = `Items(${result.CartItemsNumber})`;
    cartItems.setAttribute("number", result.CartItemsNumber.toString());
    document.getElementById(`cart-item-${id}`).innerText = numberObject.parentElement.parentElement.parentElement.parentElement.getElementsByClassName("card-title")[0].innerHTML + `(${number})`;
    document.getElementById("final-price").innerText = USDollar.format(result.CartNewPrice);
}

async function removeFromCart (id) {
    let response = await fetch(`/Product/RemoveFromCart/${id}`);
    let cartPrice = await response.json();
    let USDollar = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
    });

    let item = document.getElementById(id.toString());
    let cartItems = document.getElementById("cart-items-number");
    let itemsNumber = parseInt(cartItems.getAttribute("number")) - parseInt(item.querySelector("#dropdownMenuButton").innerText);
    
    cartItems.innerText = `Items(${itemsNumber})`;
    cartItems.setAttribute("number", itemsNumber.toString());
    item.remove();
    document.getElementById(`cart-item-${id}`).remove();
    document.getElementById("final-price").innerText = USDollar.format(cartPrice);
    if (itemsNumber === 0)
    {
        document.getElementById("cart-div").remove();        
    }
}
