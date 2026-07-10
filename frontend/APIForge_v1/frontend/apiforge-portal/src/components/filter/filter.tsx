
const Filter = () => {

    

    return (
        <>
        <h1>Filter component</h1>
        <select name="statusCode" id="statusCode">
            <option value="200">Success</option>
            <option value="500">Internal Server Error</option>
            <option value="404">Not Found</option>
        </select>
         <select name="Api" id="Api">
            <option value="products">product</option>
            <option value="orders">order</option>
        </select>
        </>
    )
}

export default Filter;