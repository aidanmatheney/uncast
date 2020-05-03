var mysql = require('mysql');
var express = require('express');
var app = express();
var multer = require('multer');
var cors = require('cors');

// Set up connection to database
var connection = mysql.createConnection({
    host: 'localhost',
    user: 'root',
    password: 'Jigjag$0',
    database: 'uncast',
  });
  

// Connect to database
connection.connect();

app.use(cors());
app.use(bodyParser.json());

var storage = multer.diskStorage({
    destination: function (req, file, cb) {
        cb(null, 'test_file_storage')
    },
    filename: function (req, file, cb) {
        cb(null, Date.now() + '-' +file.originalname )
    }
})

var upload = multer({storage: storage}).single('file');

app.post('/upload', function(req, res) {
    upload(req, res, function (err) {
        if (err instanceof multer.MulterError) {
            return res.status(500).json(err)
        } else if (err) {
            return res.status(500).json(err)
        }
   return res.status(200).send(req.file)

 })
});

app.post('/customrsspodcast', function(req, res) {
    const selectedRSS = req.body.selectedRSS;
    con.query('INSERT INTO customrsspodcast (Id, FeedUrl) VALUES (1, ?)', [selectedRSS], function(err, result) {
        console.log(selectedRSS);
        if(err) throw err;
            res.end('success');
    });
    res.end('Success');
  });

app.listen(3306, function() {
    console.log('Successfully connected to Uncast MySQL DB on port 3306.')
});