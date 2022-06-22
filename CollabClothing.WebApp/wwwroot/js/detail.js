var imgFeatures =document.querySelector('.img-feature');
var listImages = document.querySelectorAll('.list-image img');
var prevBtn = document.querySelector('.prev');
var nextBtn = document.querySelector('.next');
var currentIndex = 0;
function changeImage(index){
    document.querySelectorAll('.list-image div').forEach(function(item){
        item.classList.remove('active');
    });
    currentIndex = index;
    imgFeatures.src = listImages[index].src;
    listImages[index].parentElement.classList.add('active');
}
listImages.forEach((imgElement,index) => {
    imgElement.addEventListener('click',e =>{
        imgFeatures.style.opacity =0;
        setTimeout(() =>{
            changeImage(index);
            imgFeatures.style.opacity =1;
        },400);
       changeImage(index);
    })
})

prevBtn.addEventListener('click',e =>{
    if(currentIndex == 0){
        currentIndex = listImages.length - 1;
    }else{
        currentIndex--;
    }
    imgFeatures.style.animation ='';
    setTimeout(()=>{
        changeImage(currentIndex);
        imgFeatures.style.animation ='slideLeft 1s ease-in-out forwards';
    },200)

})

nextBtn.addEventListener('click',e =>{
    if(currentIndex == listImages.length - 1){
        currentIndex = 0;
    }else{
        currentIndex++;
    }
    imgFeatures.style.animation ='';
    setTimeout(()=>{
        changeImage(currentIndex);
        imgFeatures.style.animation ='slideRight 1s ease-in-out forwards';
    },200)

})