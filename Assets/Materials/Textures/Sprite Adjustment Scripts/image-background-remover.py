from PIL import Image

image_path = "Title_Logo.png"  # Replace with your image path
threshold = 50
margin = range(-threshold, threshold)

# open the image
img = Image.open(image_path)
img = img.convert("RGBA")

img_data = img.getdata()


def is_within_margin(px, margin):
    return (px[0] - 193) in margin and (px[1] - 154) in margin and (px[2] - 33) in margin


# check if any neighboring pixel passes the margin check
def has_neighbor_within_margin(x, y, margin):
    for dx in range(-1, 2):
        for dy in range(-1, 2):
            try:
                if (dx != 0 or dy != 0) and is_within_margin(img.getpixel((x + dx, y + dy)), margin):
                    return True
            except IndexError:
                pass
    return False

# create a new image with transparency
new_img_data = []
width, height = img.size
for y in range(height):
    for x in range(width):
        pixel = img.getpixel((x, y))
        # check if pixel color is within margin or has a neighboring pixel within the margin
        if is_within_margin(pixel, margin) or has_neighbor_within_margin(x, y, margin):
            # keep pixel as is
            new_img_data.append(pixel)
        else:
            # set pixel to fully transparent
            new_img_data.append((255, 255, 255, 0))

# update the image with the new pixel data
img.putdata(new_img_data)

# save the image as background-free PNG
img.save("background_free.png", "PNG")
print("Background-free image saved as 'background_free.png'")
