const beanUrl = "https://localhost:5001/api/beanvariety/";
const coffeeUrl = "https://localhost:5001/api/coffee";

const button = document.querySelector("#run-button");
const target = document.querySelector(".bean-variety-target");
button.addEventListener("click", () => {
    getAllBeanVarieties()
        .then(beanVarieties => {
            for(bean in beanVarieties)
            {
                target.innerHTML += BeanVarietyView(bean)
            };
        })
});

function getAllBeanVarieties() {
    return fetch(beanUrl).then(resp => resp.json());
}

const BeanVarietyView = (bean) =>
{
    return `
        <div class="bean-variety-div">
            <h3>${bean?.Name}</h3>
            <p>${bean?.Region}</p>
            <p>N${bean?.Notes}</p>
        </div>
    
    `
}