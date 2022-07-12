const BeanVarietyView = (bean) =>
{
    return `
        <div class="bean-variety-div">
            <h3>${bean.Name}</h3>
            <p>${bean.Region}</p>
            <p>N${bean?.Notes}</p>
        </div>
    
    `
}