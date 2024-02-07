const grabzit = require('./grabzit')

var client;

beforeEach(() => {
    client = new grabzit("YOUR APPLICATION KEY", "YOUR APPLICATION SECRET");
});

test('html converts to image', done => {
    function callback(error, id){
        try {
            expect(error).toBeNull();
            done();
        } catch (error) {
            done(error);
        }
    }

    client.html_to_image("<h1>Hello world</h1>");    
    client.save(null, callback);
}, 10000);

test('html converts to video', done => {
    function callback(error, id){
        try {
            expect(error).toBeNull();
            done();
        } catch (error) {
            done(error);
        }
    }

    client.html_to_video("<h1>Hello world</h1>");    
    client.save(null, callback);
}, 10000);