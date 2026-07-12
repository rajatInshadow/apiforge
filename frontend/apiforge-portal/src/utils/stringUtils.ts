export function getApiNameFromPath(path:string | null | undefined) {
    console.log('path path ',path)
    if(path!= null) {
        const parts =  path.split("/");
        console.log('parts parts ',parts, 'parts[parts.length-1 ',parts[parts.length-1])

        return parts[parts.length-1];

    }else return ""
}