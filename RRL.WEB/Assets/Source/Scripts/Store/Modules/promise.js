import $ from 'jquery'

class AskPromise  {
    require(data, callback) {
        const p = new Promise((resolve, reject) => {
            $.ajax({
                url:data.url,
                type:data.type,
                data:data.data?data.data:null,
                success(res) {
                    callback(res);
                    resolve()
                }
            })
        })
        return p
    }
}

export default AskPromise