import { Button } from 'reactstrap'

import './../assets/styles/common.css';

function News() {
  return (
    <section className="gutter-lg">
      <p className="fs-title text-start">Title</p>
      <p className="text-start mb-4">
        Donec urna sapien, porta nec condimentum id, elementum in quam. Curabitur a aliquet turpis, sagittis eleifend sem. Nam quis est eget neque euismod fringilla et sit amet mauris. Proin commodo sodales arcu, a volutpat augue facilisis et. Nam pellentesque finibus felis eget feugiat. Nulla convallis a nisl non vehicula. Aliquam erat volutpat. Vivamus id massa a lacus tincidunt tempor at eu ex...
      </p>
      <Button id="learn-more" type="button"
        name="learn-more" color="primary"
        className="float-start">
        Learn More
      </Button>
    </section>
  );
}

export default News;